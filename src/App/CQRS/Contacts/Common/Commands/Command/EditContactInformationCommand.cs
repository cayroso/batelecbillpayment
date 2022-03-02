using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.CQRS.Commands;
namespace App.CQRS.Contacts.Common.Commands.Command
{
    public sealed class EditContactInformationCommand : AbstractCommand
    {
        public EditContactInformationCommand(string correlationId, string tenantId, string userId, string contactId, string token,
            EnumContactSalutation salutation, string firstName, string middleName, string lastName, string homePhone, string mobilePhone,
            string businessPhone, string fax, string email, string website, string address, double geoX, double geoY)
            : base(correlationId, tenantId, userId)
        {
            ContactId = contactId;
            Token = token;
            Salutation = salutation;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            HomePhone = homePhone;
            MobilePhone = mobilePhone;
            BusinessPhone = businessPhone;
            Fax = fax;
            Email = email;
            Website = website;
            Address = address;
            GeoX = geoX;
            GeoY = geoY;
        }

        public string ContactId { get; }
        public string Token { get; }
        public EnumContactSalutation Salutation { get; }

        public string FirstName { get; }
        public string MiddleName { get; }
        public string LastName { get; }

        public string HomePhone { get; }
        public string MobilePhone { get; }
        public string BusinessPhone { get; }
        public string Fax { get; }

        public string Email { get; }
        public string Website { get; }

        public string Address { get; }
        public double GeoX { get; }
        public double GeoY { get; }
    }
}
