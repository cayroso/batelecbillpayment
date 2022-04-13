//using App.CQRS;
//using App.CQRS.Contacts.Common.Queries.Query;
//using Cayent.Core.CQRS.Queries;
//using Data.App.DbContext;
//using Data.App.Models.Contacts;
//using Data.App.Models.FileUploads;
//using Data.Common;
//using Data.Constants;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Web.Areas.Member.Models;
//using Web.Controllers;

//namespace Web.Areas.Manager.Controllers
//{
//    [Authorize(Policy = ApplicationRoles.MemberRoleName)]
//    [ApiController]
//    [Route("api/managers/[controller]")]
//    [Produces("application/json")]
//    public class ContactsController : BaseController
//    {
//        readonly IQueryHandlerDispatcher _queryHandlerDispatcher;
//        public ContactsController(IQueryHandlerDispatcher queryHandlerDispatcher)
//        {
//            _queryHandlerDispatcher = queryHandlerDispatcher ?? throw new ArgumentNullException(nameof(queryHandlerDispatcher));
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetContacts([FromServices] AppDbContext appDbContext, string c, int p, int s, string sf, int so)
//        {
//            var sql = from contact in appDbContext.Contacts

//                      where string.IsNullOrWhiteSpace(c)
//                            || EF.Functions.Like(contact.FirstName, $"%{c}%")
//                            || EF.Functions.Like(contact.MiddleName, $"%{c}%")
//                            || EF.Functions.Like(contact.LastName, $"%{c}%")

//                            || EF.Functions.Like(contact.Email, $"%{c}%")
//                            || EF.Functions.Like(contact.HomePhone, $"%{c}%")
//                            || EF.Functions.Like(contact.MobilePhone, $"%{c}%")
//                            || EF.Functions.Like(contact.BusinessPhone, $"%{c}%")
//                            || EF.Functions.Like(contact.Fax, $"%{c}%")

//                      orderby contact.DateUpdated descending, contact.CreatedBy descending

//                      select new
//                      {
//                          contact.ContactId,

//                          SalutationText = contact.Salutation.ToString(),
//                          contact.FirstName,
//                          contact.MiddleName,
//                          contact.LastName,

//                          Status = contact.Status,
//                          StatusText = contact.Status.ToString(),
//                          contact.Rating,

//                          contact.Email,
//                          contact.HomePhone,
//                          contact.MobilePhone,
//                          contact.BusinessPhone,
//                          contact.Fax,

//                          contact.AssignedToId,
//                          AssignedTo = contact.AssignedTo != null ? contact.AssignedTo.FirstLastName : string.Empty,

//                          Token = contact.ConcurrencyToken
//                      };

//            var dto = await sql.ToPagedItemsAsync(p, s);

//            return Ok(dto);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetContacts([FromServices] AppDbContext appDbContext, string id, System.Threading.CancellationToken cancellationToken = default)
//        {
//            var query = new GetContactByIdQuery("", TenantId, UserId, id);

//            var dto = await _queryHandlerDispatcher.HandleAsync<GetContactByIdQuery, GetContactByIdQuery.Contact>(query, cancellationToken);

//            return Ok(dto);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddContact([FromServices] AppDbContext appDbContext, [FromBody] AddContactInfo info)
//        {
//            if (ModelState.IsValid)
//            {
//                var data = new Contact
//                {
//                    ContactId = GuidStr(),
//                    Title = info.Title,
//                    FirstName = info.FirstName,
//                    MiddleName = info.MiddleName,
//                    LastName = info.LastName,
//                    Salutation = info.Salutation,
//                    HomePhone = info.HomePhone,
//                    MobilePhone = info.MobilePhone,
//                    BusinessPhone = info.BusinessPhone,
//                    Fax = info.Fax,
//                    Email = info.Email,
//                    Website = info.Website,
//                    Address = info.Address,
//                    GeoX = info.GeoX,
//                    GeoY = info.GeoY,

//                    ReferralSource = info.ReferralSource,
//                    Company = info.Company,
//                    Industry = info.Industry,
//                    AnnualRevenue = info.AnnualRevenue,
//                    Rating = info.Rating,

//                    Status = Data.Enums.EnumContactStatus.Lead,
//                    CreatedById = UserId,
//                    AssignedToId = UserId,
//                    DateCreated = DateTime.UtcNow,
//                    DateOfInitialContact = info.DateOfInitialContact,
//                    DateUpdated = DateTime.UtcNow,

//                };

//                await appDbContext.AddAsync(data);

//                await appDbContext.SaveChangesAsync();

//                return Ok(data.ContactId);
//            }

//            return BadRequest();
//        }

//        [HttpPut("{id}/assign/{userId}/{token}")]
//        public async Task<IActionResult> Assign([FromServices] AppDbContext dbContext, string id, string userId, string token)
//        {
//            var data = await dbContext.Contacts.Include(e => e.Tasks).FirstOrDefaultAsync(e => e.ContactId == id);

//            data.ThrowIfNullOrAlreadyUpdated(token, GuidStr());

//            data.AssignedToId = userId;
            
//            foreach(var task in data.Tasks)
//            {
//                task.UserId = userId;
//                task.DateAssigned = DateTime.UtcNow;
//            }

//            await dbContext.SaveChangesAsync();

//            return Ok();
//        }

//        #region Attachments

//        [HttpPost("add-note")]
//        public async Task<IActionResult> AddAttachment([FromServices] AppDbContext appDbContext, [FromBody] AddAttachmentInfo info)
//        {

//            if (ModelState.IsValid)
//            {
//                var data = await appDbContext.Contacts.FirstOrDefaultAsync(e => e.ContactId == info.ContactId);

//                data.ThrowIfNull();

//                data.Attachments.Add(new ContactAttachment
//                {
//                    ContactAttachmentId = GuidStr(),
//                    AttachmentType = Data.Enums.EnumContactAttachmentType.Note,
//                    Title = info.Title,
//                    Content = info.Content,
//                    DateCreated = DateTime.UtcNow,
//                    DateUpdated = DateTime.UtcNow,
//                });


//                await appDbContext.SaveChangesAsync();

//                return Ok();
//            }

//            return BadRequest();

//        }

//        [HttpPut("edit-note")]
//        public async Task<IActionResult> EditAttachment([FromServices] AppDbContext appDbContext, [FromBody] EditAttachmentInfo info)
//        {

//            if (ModelState.IsValid)
//            {
//                var data = await appDbContext.ContactAttachments.FirstOrDefaultAsync(e => e.ContactAttachmentId == info.ContactAttachmentId);

//                data.ThrowIfNullOrAlreadyUpdated(info.Token, GuidStr());

//                data.Title = info.Title;
//                data.Content = info.Content;
//                data.DateUpdated = DateTime.UtcNow;

//                await appDbContext.SaveChangesAsync();

//                return Ok(data.ConcurrencyToken);
//            }

//            return BadRequest();

//        }

//        [HttpDelete("delete-attachment/{id}/{token}")]
//        public async Task<IActionResult> DeleteAttachment([FromServices] AppDbContext appDbContext, string id, string token)
//        {

//            if (ModelState.IsValid)
//            {
//                var data = await appDbContext.ContactAttachments.Include(e => e.FileUpload).FirstOrDefaultAsync(e => e.ContactAttachmentId == id);

//                data.ThrowIfNullOrAlreadyUpdated(token, GuidStr());

//                if (data.AttachmentType == Data.Enums.EnumContactAttachmentType.File && data.FileUpload != null)
//                {
//                    appDbContext.Remove(data.FileUpload);
//                }

//                appDbContext.Remove(data);

//                await appDbContext.SaveChangesAsync();

//                return Ok();
//            }

//            return BadRequest();

//        }

//        [HttpPost("add-file")]
//        public async Task<IActionResult> AddAttachment([FromServices] AppDbContext appDbContext)
//        {
//            var infoFile = Request.Form.Files.FirstOrDefault(e => e.Name == "info");
//            var attachmentFile = Request.Form.Files.FirstOrDefault(e => e.Name == "files");

//            if (infoFile != null && infoFile.Length > 0)
//            {
//                var streamReader = new StreamReader(infoFile.OpenReadStream());

//                var infoJson = streamReader.ReadToEnd();

//                var info = JsonConvert.DeserializeObject<AddFileAttachmentInfo>(infoJson);

//                var data = await appDbContext.Contacts.FirstOrDefaultAsync(e => e.ContactId == info.ContactId);

//                data.ThrowIfNullOrAlreadyUpdated(info.Token, GuidStr());

//                var fileUpload = new FileUpload
//                {
//                    FileUploadId = GuidStr(),
//                    Url = info.ImageLink,
//                    FileName = info.ImageName,
//                    DateCreated = DateTime.UtcNow,
//                };

//                if (attachmentFile != null)
//                {
//                    var bytes = new byte[attachmentFile.Length];

//                    using (var stream = attachmentFile.OpenReadStream())
//                    {
//                        stream.Read(bytes);
//                    }

//                    var fileUploadId = GuidStr();

//                    fileUpload = new FileUpload
//                    {
//                        FileUploadId = fileUploadId,
//                        FileName = attachmentFile.FileName,
//                        ContentDisposition = attachmentFile.ContentDisposition,
//                        ContentType = attachmentFile.ContentType,
//                        Content = bytes,
//                        Length = attachmentFile.Length,
//                        DateCreated = DateTime.UtcNow,
//                        Url = $"api/files/{TenantId}/{fileUploadId}",
//                    };
//                }

//                var attachment = new ContactAttachment
//                {
//                    ContactAttachmentId = GuidStr(),
//                    AttachmentType = Data.Enums.EnumContactAttachmentType.File,
//                    FileUpload = fileUpload,
//                    DateCreated = DateTime.UtcNow,
//                    DateUpdated = DateTime.UtcNow,
//                };

//                data.Attachments.Add(attachment);

//                await appDbContext.SaveChangesAsync();

//                return Ok();
//            }

//            return BadRequest();
//        }

//        #endregion

//        #region Edits

//        [HttpPut("edit-contact-information")]
//        public async Task<IActionResult> EditContactInformation([FromServices] AppDbContext appDbContext, [FromBody] EditContactInfo info)
//        {
//            if (ModelState.IsValid)
//            {
//                var data = await appDbContext.Contacts.FirstOrDefaultAsync(e => e.ContactId == info.ContactId);

//                data.ThrowIfNullOrAlreadyUpdated(info.Token, GuidStr());

//                data.Salutation = info.Salutation;
//                data.FirstName = info.FirstName;
//                data.MiddleName = info.MiddleName;
//                data.LastName = info.LastName;

//                data.HomePhone = info.HomePhone;
//                data.MobilePhone = info.MobilePhone;
//                data.BusinessPhone = info.BusinessPhone;
//                data.Fax = info.Fax;

//                data.Email = info.Email;
//                data.Website = info.Website;
//                data.Address = info.Address;


//                data.DateUpdated = DateTime.UtcNow;

//                await appDbContext.SaveChangesAsync();

//                return Ok();
//            }

//            return BadRequest();

//        }

//        [HttpPut("edit-work-information")]
//        public async Task<IActionResult> EditWorkInformation([FromServices] AppDbContext appDbContext, [FromBody] EditWorkInfo info)
//        {
//            if (ModelState.IsValid)
//            {
//                var data = await appDbContext.Contacts.FirstOrDefaultAsync(e => e.ContactId == info.ContactId);

//                data.ThrowIfNullOrAlreadyUpdated(info.Token, GuidStr());

//                data.Title = info.Title;
//                data.Company = info.Company;
//                data.Industry = info.Industry;
//                data.AnnualRevenue = info.AnnualRevenue;
//                data.DateUpdated = DateTime.UtcNow;

//                await appDbContext.SaveChangesAsync();

//                return Ok();
//            }

//            return BadRequest();

//        }

//        [HttpPut("edit-system-information")]
//        public async Task<IActionResult> EditSystemInformation([FromServices] AppDbContext appDbContext, [FromBody] EditSystemInfo info)
//        {
//            if (ModelState.IsValid)
//            {
//                var data = await appDbContext.Contacts.FirstOrDefaultAsync(e => e.ContactId == info.ContactId);

//                data.ThrowIfNullOrAlreadyUpdated(info.Token, GuidStr());

//                data.Status = info.Status;
//                data.ReferralSource = info.ReferralSource;
//                data.DateOfInitialContact = info.DateOfInitialContact;
//                data.Rating = info.Rating;
//                data.DateUpdated = DateTime.UtcNow;

//                await appDbContext.SaveChangesAsync();

//                return Ok();
//            }

//            return BadRequest();

//        }

//        #endregion
//    }
//}
