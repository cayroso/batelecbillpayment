using Cayent.Core.CQRS.Queries;
using Cayent.Core.Common.Extensions;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.CQRS.Contacts.Common.Queries.Query
{
    public sealed class GetContactByIdQuery : AbstractQuery<GetContactByIdQuery.Contact>
    {
        public GetContactByIdQuery(string correlationId, string tenantId, string userId, string contactId)
            : base(correlationId, tenantId, userId)
        {
            ContactId = contactId;
        }

        public string ContactId { get; }

        public class Contact
        {
            public string ContactId { get; set; }

            public EnumContactSalutation Salutation { get; set; }
            public string SalutationText => Salutation.ToString();

            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }

            public string Title { get; set; }
            public string Company { get; set; }
            public string Industry { get; set; }
            public decimal AnnualRevenue { get; set; }
            public int Rating { get; set; }
            public string HomePhone { get; set; }
            public string MobilePhone { get; set; }
            public string BusinessPhone { get; set; }
            public string Fax { get; set; }

            public string Email { get; set; }
            public string Website { get; set; }

            public string Address { get; set; }
            public double GeoX { get; set; }
            public double GeoY { get; set; }

            public string Token { get; set; }

            public SystemInformation SystemInformation { get; set; }

            //public IEnumerable<ContactNote> Notes { get; set; }
            public IEnumerable<ContactAttachment> Attachments { get; set; }
            public IEnumerable<Task> Tasks { get; set; }

            public IEnumerable<Activity> Activities { get; set; }
        }

        public class SystemInformation
        {
            public EnumContactStatus Status { get; set; }
            public string StatusText => Status.ToString();
            public int Rating { get; set; }
            public string ReferralSource { get; set; }

            public string CreatedBy { get; set; }
            public string AssignedToId { get; set; }
            public string AssignedTo { get; set; }

            DateTime _dateOfInitialContact;
            public DateTime DateOfInitialContact
            {
                get => _dateOfInitialContact;
                set => _dateOfInitialContact = value.AsUtc();
            }

            DateTime _dateCreated;
            public DateTime DateCreated
            {
                get => _dateCreated;
                set => _dateCreated = value.AsUtc();
            }

            DateTime _dateUpdated;
            public DateTime DateUpdated
            {
                get => _dateUpdated;
                set => _dateUpdated = value.AsUtc();
            }
        }

        public class ContactNote
        {
            public string ContactNoteId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            DateTime _dateCreated;
            public DateTime DateCreated
            {
                get => _dateCreated;
                set => _dateCreated = value.AsUtc();
            }

            DateTime _dateUpdated;
            public DateTime DateUpdated
            {
                get => _dateUpdated;
                set => _dateUpdated = value.AsUtc();
            }

            public string Token { get; set; }
        }

        public class ContactAttachment
        {
            public string ContactAttachmentId { get; set; }

            public EnumContactAttachmentType AttachmentType { get; set; }
            public string AttachmentTypeText => AttachmentType.ToString();

            public string Url { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public long Length { get; set; }

            public string Title { get; set; }
            public string Content { get; set; }

            public bool IsDeleted { get; set; }

            DateTime _dateCreated;
            public DateTime DateCreated
            {
                get => _dateCreated;
                set => _dateCreated = value.AsUtc();
            }

            DateTime _dateUpdated;
            public DateTime DateUpdated
            {
                get => _dateUpdated;
                set => _dateUpdated = value.AsUtc();
            }

            public string Token { get; set; }
        }

        public class Task
        {
            public string TaskId { get; set; }

            public EnumTaskType TaskType { get; set; }
            public string TaskTypeText => TaskType.ToString();

            public EnumTaskStatus TaskStatus { get; set; }
            public string TaskStatusText => TaskStatus.ToString();

            public string Title { get; set; }
            public string Description { get; set; }


            DateTime _dateCreated;
            public DateTime DateCreated
            {
                get => _dateCreated;
                set => _dateCreated = value.AsUtc();
            }

            DateTime _dateAssigned;
            public DateTime DateAssigned
            {
                get => _dateAssigned;
                set => _dateAssigned = value.AsUtc();
            }

            DateTime _dateCompleted;
            public DateTime DateCompleted
            {
                get => _dateCompleted;
                set => _dateCompleted = value.AsUtc();
            }

            DateTime _dateActualCompleted;
            public DateTime DateActualCompleted
            {
                get => _dateActualCompleted;
                set => _dateActualCompleted = value.AsUtc();
            }

            DateTime _dateDeleted;
            public DateTime DateDeleted
            {
                get => _dateDeleted;
                set => _dateDeleted = value.AsUtc();
            }

            public bool IsDeleted => _dateDeleted < DateTime.UtcNow;

            public string Token { get; set; }

            public IEnumerable<TaskItem> TaskItems { get; set; }
        }

        public class TaskItem
        {
            public string TaskItemId { get; set; }
            public string Title { get; set; }
            public bool IsDone { get; set; }

            //DateTime _dateCompleted;
            //public DateTime DateCompleted
            //{
            //    get => _dateCompleted;
            //    set => _dateCompleted = value.AsUtc();
            //}
            public string Token { get; set; }
        }

        public class Activity
        {
            public string Description { get; set; }

            public string User { get; set; }

            DateTime _dateCreated;
            public DateTime DateCreated
            {
                get => _dateCreated;
                set => _dateCreated = value.AsUtc();
            }
        }
    }
}
