using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.CQRS.Commands;
namespace App.CQRS.Contacts.Common.Commands.Command
{
    public sealed class EditContactWorkInformationCommand : AbstractCommand
    {
        public EditContactWorkInformationCommand(string correlationId, string tenantId, string userId,
            string contactId, string token, string title, string company, string industry, decimal annualRevenue)
            : base(correlationId, tenantId, userId)
        {
            ContactId = contactId;
            Token = token;
            Title = title;
            Company = company;
            Industry = industry;
            AnnualRevenue = annualRevenue;
        }

        public string ContactId { get; }
        public string Token { get; }
        public string Title { get; }
        public string Company { get; }
        public string Industry { get; }
        public decimal AnnualRevenue { get; }
    }
}
