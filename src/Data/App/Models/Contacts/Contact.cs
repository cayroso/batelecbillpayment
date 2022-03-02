using Cayent.Core.Common.Extensions;
using Data.App.Models.Activities;
using Data.App.Models.Teams;
using Data.App.Models.Users;
using Data.App.Models.Users.UserTasks;
using Data.Common;
using Data.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Contacts
{
    public abstract class ContactBase
    {
        public string CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public string AssignedToId { get; set; }
        public virtual User AssignedTo { get; set; }

        public EnumContactStatus Status { get; set; }
        public EnumContactSalutation Salutation { get; set; }
        public EnumContactGender Gender { get; set; }
        public EnumContactCivilStatus CivilStatus { get; set; }


        DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get => _dateOfBirth.AsUtc();
            set => _dateOfBirth = value.Truncate();
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string BirthPlace { get; set; }

        public string TIN { get; set; }
        public string GSIS { get; set; }
        public string SSS { get; set; }


        public string HomePhone { get; set; }
        public string BusinessPhone { get; set; }
        public string MobilePhone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        public string Address { get; set; }
        public double GeoX { get; set; }
        public double GeoY { get; set; }

        public bool Smoker { get; set; }
        public bool AlcoholDrinker { get; set; }


        public int Rating { get; set; }

        DateTime _dateOfInitialContact;
        public DateTime DateOfInitialContact
        {
            get => _dateOfInitialContact.AsUtc();
            set => _dateOfInitialContact = value.Truncate();
        }

        public string ReferralSource { get; set; }

        public string Title { get; set; }
        public string Company { get; set; }
        public string Industry { get; set; }
        public decimal AnnualRevenue { get; set; }

        // Lead Details
        //  InterestedIn
        //  Source
        //  

        DateTime _dateCreated = DateTime.UtcNow.Truncate();
        public DateTime DateCreated
        {
            get => _dateCreated.AsUtc();
            set => _dateCreated = value.Truncate();
        }

        DateTime _dateUpdated = DateTime.UtcNow.Truncate();
        public DateTime DateUpdated
        {
            get => _dateUpdated.AsUtc();
            set => _dateUpdated = value.Truncate();
        }

        DateTime _dateDeleted = DateTime.MaxValue.Truncate();
        public DateTime DateDeleted
        {
            get => _dateDeleted.AsUtc();
            set => _dateDeleted = value.Truncate();
        }
    }

    public class Contact : ContactBase
    {
        public string ContactId { get; set; }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<ContactAttachment> Attachments { get; set; } = new List<ContactAttachment>();
        public virtual ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();
        public virtual ICollection<ContactActivity> Activities { get; set; } = new List<ContactActivity>();
    }


    public class ContactAudit : ContactBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AuditId { get; set; }
        public EnumAuditAction AuditAction { get; set; }

        public string AuditUserId { get; set; }
        public virtual User AuditUser { get; set; }

        DateTime _dateLog = DateTime.UtcNow.Truncate().AsUtc();
        public DateTime DateLog
        {
            get => _dateLog.AsUtc();
            set => _dateLog = value.Truncate();
        }

        public string EntityId { get; set; }
    }
}
