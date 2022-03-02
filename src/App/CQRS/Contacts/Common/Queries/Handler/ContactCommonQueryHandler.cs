using App.CQRS.Contacts.Common.Queries.Query;
using Cayent.Core.CQRS.Queries;
using Data.App.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.CQRS.Contacts.Common.Queries.Handler
{
    public sealed class ContactCommonQueryHandler :
        IQueryHandler<GetContactByIdQuery, GetContactByIdQuery.Contact>
    {
        readonly AppDbContext _dbContext;
        public ContactCommonQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        async Task<GetContactByIdQuery.Contact> IQueryHandler<GetContactByIdQuery, GetContactByIdQuery.Contact>.HandleAsync(GetContactByIdQuery query, System.Threading.CancellationToken cancellationToken)
        {
            var sql = from c in _dbContext.Contacts.AsNoTracking()

                      where c.ContactId == query.ContactId

                      select new GetContactByIdQuery.Contact
                      {
                          ContactId = c.ContactId,

                          Salutation = c.Salutation,
                          FirstName = c.FirstName,
                          MiddleName = c.MiddleName,
                          LastName = c.LastName,

                          HomePhone = c.HomePhone,
                          MobilePhone = c.MobilePhone,
                          BusinessPhone = c.BusinessPhone,
                          Fax = c.Fax,

                          Email = c.Email,
                          Website = c.Website,

                          Address = c.Address,
                          GeoX = c.GeoX,
                          GeoY = c.GeoY,

                          Title = c.Title,
                          Company = c.Company,
                          AnnualRevenue = c.AnnualRevenue,
                          Industry = c.Industry,
                          Rating = c.Rating,


                          Token = c.ConcurrencyToken,

                          SystemInformation = new GetContactByIdQuery.SystemInformation
                          {
                              CreatedBy = c.CreatedBy != null ? c.CreatedBy.FirstLastName : string.Empty,
                              AssignedToId = c.AssignedToId,
                              AssignedTo = c.AssignedTo != null ? c.AssignedTo.FirstLastName : string.Empty,
                              DateOfInitialContact = c.DateOfInitialContact,
                              DateCreated = c.DateCreated,
                              DateUpdated = c.DateUpdated,
                              Rating = c.Rating,
                              ReferralSource = c.ReferralSource,
                              Status = c.Status,
                          },

                          //Notes = c.Notes.OrderByDescending(e => e.DateUpdated).ThenByDescending(e => e.DateCreated).Select(e => new GetContactByIdQuery.ContactNote
                          //{
                          //    ContactNoteId = e.ContactNoteId,
                          //    Title = e.Title,
                          //    Content = e.Content,
                          //    DateCreated = e.DateCreated,
                          //    DateUpdated = e.DateUpdated,
                          //    Token = e.ConcurrencyToken
                          //}),

                          Attachments = c.Attachments
                          //.Where(e => e.DateDeleted > DateTime.UtcNow)
                          .OrderByDescending(e => e.DateUpdated)
                          .ThenByDescending(e => e.DateCreated)
                          .Select(e => new GetContactByIdQuery.ContactAttachment
                          {
                              ContactAttachmentId = e.ContactAttachmentId,
                              AttachmentType = e.AttachmentType,

                              Url = e.FileUpload == null ? string.Empty : e.FileUpload.Url,
                              ContentType = e.FileUpload == null ? string.Empty : e.FileUpload.ContentType,
                              FileName = e.FileUpload == null ? string.Empty : e.FileUpload.FileName,
                              Length = e.FileUpload == null ? 0 : e.FileUpload.Length,

                              Title = e.Title,
                              Content = e.Content,
                              IsDeleted = e.DateDeleted < DateTime.UtcNow,
                              DateCreated = e.DateCreated,
                              DateUpdated = e.DateUpdated,

                              Token = e.ConcurrencyToken
                          }),

                          Tasks = c.Tasks.Where(e => e.UserId == query.UserId)
                                    .OrderBy(e => e.DateCompleted).ThenByDescending(e => e.DateAssigned).ThenByDescending(e => e.DateCreated)
                                    .Select(e => new GetContactByIdQuery.Task
                                    {
                                        TaskId = e.UserTaskId,
                                        Title = e.Title,
                                        Description = e.Description,
                                        TaskStatus = e.Status,
                                        TaskType = e.Type,
                                        DateCreated = e.DateCreated,
                                        DateAssigned = e.DateAssigned,
                                        DateCompleted = e.DateCompleted,
                                        DateActualCompleted = e.DateActualCompleted,
                                        DateDeleted = e.DateDeleted,
                                        Token = e.ConcurrencyToken,
                                        TaskItems = e.UserTaskItems.OrderBy(e => e.Number).Select(p => new GetContactByIdQuery.TaskItem
                                        {
                                            TaskItemId = p.UserTaskItemId,
                                            Title = p.Title,
                                            IsDone = p.IsDone,
                                            Token = p.ConcurrencyToken
                                        })
                                    }),
                          Activities = c.Activities.OrderByDescending(e => e.Activity.DateCreated).Select(e => new GetContactByIdQuery.Activity
                          {
                              Description = e.Activity.Description,
                              DateCreated = e.Activity.DateCreated,
                              User = e.Activity.User.FirstLastName
                          })

                      };

            var dto = await sql.FirstOrDefaultAsync();

            return dto;

        }
    }
}
