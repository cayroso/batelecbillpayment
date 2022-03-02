using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.CQRS.Commands;
namespace App.CQRS.Contacts.Common.Commands.Command
{
    public sealed class AddContactCommand : AbstractCommand
    {
        public AddContactCommand(string correlationId, string tenantId, string userId,
            string contactId, string title, string firstName, string middleName, string lastName,
            EnumContactStatus status, EnumContactSalutation salutation, EnumContactGender gender, EnumContactCivilStatus civilStatus,
            DateTime dateOfBirth, string birthPlace, string tin, string sss, string gsis, bool smoker, bool alcoholDrinker,
            string homePhone, string mobilePhone, string businessPhone, string fax,
            string email, string website, string referralSource, int rating, string company, string industry, decimal annualRevenue,
            DateTime dateOfInitialContact, string address, double geoX, double geoY)
            : base(correlationId, tenantId, userId)
        {
            ContactId = contactId;
            Title = title;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Salutation = salutation;
            HomePhone = homePhone;
            MobilePhone = mobilePhone;
            BusinessPhone = businessPhone;
            Fax = fax;
            Email = email;
            Website = website;
            ReferralSource = referralSource;
            Rating = rating;
            Company = company;
            Industry = industry;
            AnnualRevenue = annualRevenue;
            DateOfInitialContact = dateOfInitialContact;
            Address = address;
            GeoX = geoX;
            GeoY = geoY;
        }

        public string ContactId { get; }
        public string Title { get; }
        public string FirstName { get; }
        public string MiddleName { get; }
        public string LastName { get; }

        public EnumContactStatus Status { get; }
        public EnumContactSalutation Salutation { get; }
        public EnumContactGender Gender { get;  }
        public EnumContactCivilStatus CivilStatus { get;  }

        public DateTime DateOfBirth { get; }
        public string BirthPlace { get; }
        public string TIN { get;  }
        public string GSIS { get;  }
        public string SSS { get;  }

        public bool Smoker { get; }
        public bool AlcoholDrinker { get; }

        public string HomePhone { get; }
        public string MobilePhone { get; }
        public string BusinessPhone { get; }
        public string Fax { get; }

        public string Email { get; }
        public string Website { get; }

        public string ReferralSource { get; }
        public int Rating { get; }
        public string Company { get; }
        public string Industry { get; }
        public decimal AnnualRevenue { get; }
        public DateTime DateOfInitialContact { get; }

        public string Address { get; }
        public double GeoX { get; }
        public double GeoY { get; }
    }
}
