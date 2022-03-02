using App.CQRS.Contacts.Common.Commands.Command;
using App.Services;
using Cayent.Core.CQRS.Commands;
using Cayent.Core.CQRS.Services;
using Data.App.DbContext;
using Data.App.Models.Activities;
using Data.App.Models.Contacts;
using Data.App.Models.FileUploads;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.CQRS.Contacts.Common.Commands.Handler
{
    public sealed class ContactCommonCommandHandler :
        ICommandHandler<AddContactAttachmentFileCommand>,
        ICommandHandler<AddContactAttachmentNoteCommand>,
        ICommandHandler<AddContactCommand>,
        ICommandHandler<DeleteContactAttachmentCommand>,
        ICommandHandler<EditContactAttachmentNoteCommand>,
        ICommandHandler<EditContactInformationCommand>,
        ICommandHandler<EditContactSystemInformationCommand>,
        ICommandHandler<EditContactWorkInformationCommand>

    {
        readonly AppDbContext _dbContext;
        readonly ISequentialGuidGenerator _guidGenerator;
        public ContactCommonCommandHandler(AppDbContext dbContext, ISequentialGuidGenerator guidGenerator)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _guidGenerator = guidGenerator ?? throw new ArgumentNullException(nameof(guidGenerator));
        }

        async Task ICommandHandler<AddContactAttachmentFileCommand>.HandleAsync(AddContactAttachmentFileCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var contact = await _dbContext.Contacts.FirstOrDefaultAsync(e => e.ContactId == command.ContactId);

            contact.ThrowIfNullOrAlreadyUpdated(command.Token, _guidGenerator.NewId());

            var fileUpload = new FileUpload
            {
                FileUploadId = command.FileUploadId,
                FileName = command.FileName,
                ContentDisposition = command.ContentDisposition,
                ContentType = command.ContentType,
                Content = command.Content,
                Length = command.Length,
                Url = command.Url,
            };

            var attachment = new ContactAttachment
            {
                ContactAttachmentId = command.ContactAttachmentId,
                ContactId = command.ContactId,
                AttachmentType = EnumContactAttachmentType.File,
                FileUpload = fileUpload,
            };

            var audit = attachment.NewAudit(EnumAuditAction.Add, command.UserId);

            contact.DateUpdated = DateTime.Now;

            var activity = CreateNewActivity(command.ContactId, command.UserId, EnumActivityEntityType.Contact, $"File attachment with filename=\"{attachment.FileUpload.FileName}\" added.");

            await _dbContext.AddRangeAsync(attachment, audit, activity);

            await _dbContext.SaveChangesAsync();
        }

        async Task ICommandHandler<AddContactAttachmentNoteCommand>.HandleAsync(AddContactAttachmentNoteCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var contact = await _dbContext.Contacts.FirstOrDefaultAsync(e => e.ContactId == command.ContactId);

            contact.ThrowIfNullOrAlreadyUpdated(command.Token, _guidGenerator.NewId());

            var attachment = new ContactAttachment
            {
                ContactAttachmentId = command.ContactAttachmentId,
                ContactId = command.ContactId,
                AttachmentType = EnumContactAttachmentType.Note,
                Title = command.Title,
                Content = command.Content
            };

            var audit = attachment.NewAudit(EnumAuditAction.Add, command.UserId);

            var activity = CreateNewActivity(command.ContactId, command.UserId, EnumActivityEntityType.Contact, $"Note attachment with title=\"{attachment.Title}\" added.");

            await _dbContext.AddRangeAsync(attachment, audit, activity);

            await _dbContext.SaveChangesAsync();
        }

        async Task ICommandHandler<AddContactCommand>.HandleAsync(AddContactCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var data = new Contact
            {
                ContactId = command.ContactId,
                Title = command.Title,
                FirstName = command.FirstName,
                MiddleName = command.MiddleName,
                LastName = command.LastName,
                HomePhone = command.HomePhone,
                MobilePhone = command.MobilePhone,
                BusinessPhone = command.BusinessPhone,
                Fax = command.Fax,
                AnnualRevenue = command.AnnualRevenue,
                Address = command.Address,
                GeoX = command.GeoX,
                GeoY = command.GeoY,
                Industry = command.Industry,
                Rating = command.Rating,
                ReferralSource = command.ReferralSource,
                Salutation = command.Salutation,
                Status = Data.Enums.EnumContactStatus.Prospect,
                AssignedToId = command.UserId,
                Company = command.Company,
                DateOfInitialContact = command.DateOfInitialContact,
                Email = command.Email,
                Website = command.Website,

                CreatedById = command.UserId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            var audit = data.NewAudit(EnumAuditAction.Add, command.UserId, DateTime.UtcNow);

            var activity = CreateNewActivity(command.ContactId, command.UserId, EnumActivityEntityType.Contact, $"Contact created.");

            await _dbContext.AddRangeAsync(data, audit, activity);

            await _dbContext.SaveChangesAsync();
        }

        async Task ICommandHandler<DeleteContactAttachmentCommand>.HandleAsync(DeleteContactAttachmentCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var attachment = await _dbContext.ContactAttachments
                .Include(e => e.Contact)
                .Include(e => e.FileUpload)
                .Include(e => e.Audit)
                .FirstOrDefaultAsync(e => e.ContactAttachmentId == command.ContactAttachmentId);

            attachment.ThrowIfNullOrAlreadyUpdated(command.Token, _guidGenerator.NewId());

            if (command.Purge)
            {
                if (attachment.FileUpload != null)
                {
                    _dbContext.Remove(attachment.FileUpload);
                }
                _dbContext.RemoveRange(attachment.Audit);
                _dbContext.Remove(attachment);
            }
            else
            {
                attachment.Contact.DateUpdated = attachment.DateDeleted = DateTime.UtcNow;

                var audit = attachment.NewAudit(EnumAuditAction.Delete, command.UserId);

                var desc = $"Contact note attachment with title=\"{attachment.Title}\" was archived.";

                if (attachment.AttachmentType == EnumContactAttachmentType.File && attachment.FileUpload != null)
                {
                    desc = $"Contact file attachment with filename=\"{attachment.FileUpload.FileName}\" was archived.";
                }

                var activity = CreateNewActivity(attachment.ContactId, command.UserId, EnumActivityEntityType.Contact, desc);

                await _dbContext.AddRangeAsync(audit, activity);
            }

            await _dbContext.SaveChangesAsync();
        }

        async Task ICommandHandler<EditContactAttachmentNoteCommand>.HandleAsync(EditContactAttachmentNoteCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var attachment = await _dbContext.ContactAttachments.Include(e => e.Contact).FirstOrDefaultAsync(e => e.ContactAttachmentId == command.ContactAttachmentId);

            attachment.ThrowIfNullOrAlreadyUpdated(command.Token, _guidGenerator.NewId());

            var clone = attachment.Clone();

            attachment.Title = command.Title;
            attachment.Content = command.Content;

            if (!attachment.HasChanges(clone))
                return;

            attachment.Contact.DateUpdated = attachment.DateUpdated = DateTime.UtcNow;

            var audit = attachment.NewAudit(EnumAuditAction.Edit, command.UserId);

            var activity = CreateNewActivity(attachment.ContactId, command.UserId, EnumActivityEntityType.Contact,
                $"Note attachment with title=\"{attachment.Title}\" updated.");

            await _dbContext.AddRangeAsync(audit, activity);

            await _dbContext.SaveChangesAsync();
        }

        async Task ICommandHandler<EditContactInformationCommand>.HandleAsync(EditContactInformationCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var contact = await _dbContext.Contacts.FirstOrDefaultAsync(e => e.ContactId == command.ContactId);

            contact.ThrowIfNullOrAlreadyUpdated(command.Token, _guidGenerator.NewId());

            var clone = contact.Clone();

            contact.Salutation = command.Salutation;
            contact.FirstName = command.FirstName;
            contact.MiddleName = command.MiddleName;
            contact.LastName = command.LastName;
            contact.HomePhone = command.HomePhone;
            contact.MobilePhone = command.MobilePhone;
            contact.BusinessPhone = command.BusinessPhone;
            contact.Fax = command.Fax;

            contact.Email = command.Email;
            contact.Website = command.Website;

            contact.Address = command.Address;
            contact.GeoX = command.GeoX;
            contact.GeoY = command.GeoY;

            if (!contact.HasChanges(clone))
                return;

            contact.DateUpdated = DateTime.UtcNow;

            var audit = contact.NewAudit(EnumAuditAction.Edit, command.UserId, DateTime.UtcNow);

            var activity = CreateNewActivity(contact.ContactId, command.UserId, EnumActivityEntityType.Contact, $"Contact information updated.");

            await _dbContext.AddRangeAsync(audit, activity);

            await _dbContext.SaveChangesAsync();
        }

        async Task ICommandHandler<EditContactSystemInformationCommand>.HandleAsync(EditContactSystemInformationCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var contact = await _dbContext.Contacts.FirstOrDefaultAsync(e => e.ContactId == command.ContactId);

            contact.ThrowIfNullOrAlreadyUpdated(command.Token, _guidGenerator.NewId());

            var clone = contact.Clone();

            contact.Status = command.Status;
            contact.ReferralSource = command.ReferralSource;
            contact.DateOfInitialContact = command.DateOfInitialContact;
            contact.Rating = command.Rating;

            if (!contact.HasChanges(clone))
                return;

            contact.DateUpdated = DateTime.UtcNow;

            var audit = contact.NewAudit(EnumAuditAction.Edit, command.UserId, DateTime.UtcNow);

            var activity = CreateNewActivity(contact.ContactId, command.UserId, EnumActivityEntityType.Contact, $"Contact system updated.");

            await _dbContext.AddRangeAsync(audit, activity);

            await _dbContext.SaveChangesAsync();
        }

        async Task ICommandHandler<EditContactWorkInformationCommand>.HandleAsync(EditContactWorkInformationCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var contact = await _dbContext.Contacts.FirstOrDefaultAsync(e => e.ContactId == command.ContactId);

            contact.ThrowIfNullOrAlreadyUpdated(command.Token, _guidGenerator.NewId());

            var clone = contact.Clone();

            contact.Title = command.Title;
            contact.Company = command.Company;
            contact.Industry = command.Industry;
            contact.AnnualRevenue = command.AnnualRevenue;

            if (!contact.HasChanges(clone))
                return;

            contact.DateUpdated = DateTime.UtcNow;

            var audit = contact.NewAudit(EnumAuditAction.Edit, command.UserId, DateTime.UtcNow);

            var activity = CreateNewActivity(contact.ContactId, command.UserId, EnumActivityEntityType.Contact, $"Contact work updated.");

            await _dbContext.AddRangeAsync(audit, activity);

            await _dbContext.SaveChangesAsync();
        }

        ContactActivity CreateNewActivity(string contactId, string userId, EnumActivityEntityType activityEntityType, string description)
        {
            var activity = new Activity
            {
                ActivityId = _guidGenerator.NewId(),
                ActivityEntityType = activityEntityType,
                EntityId = contactId,
                UserId = userId,
                Description = description,
                DateCreated = DateTime.UtcNow
            };

            var contactActivity = new ContactActivity
            {
                ContactId = contactId,
                Activity = activity,
                ActivityId = activity.ActivityId,
            };

            return contactActivity;
        }

    }
}
