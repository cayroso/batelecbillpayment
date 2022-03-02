using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.CQRS.Commands;
namespace App.CQRS.Contacts.Common.Commands.Command
{
    public sealed class EditContactSystemInformationCommand : AbstractCommand
    {
        public EditContactSystemInformationCommand(string correlationId, string tenantId, string userId,
            string contactId, string token, EnumContactStatus status, string referralSource, DateTime dateOfInitialContact, int rating)
            : base(correlationId, tenantId, userId)
        {
            ContactId = contactId;
            Token = token;
            Status = status;
            ReferralSource = referralSource;
            DateOfInitialContact = dateOfInitialContact;
            Rating = rating;
        }

        public string ContactId { get; }
        public string Token { get; }
        public EnumContactStatus Status { get; }
        public string ReferralSource { get; }
        public DateTime DateOfInitialContact { get; }
        public int Rating { get; }
    }
}
