using Cayent.Core.CQRS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.CQRS.Users.Common.Queries.Query
{
    public sealed class GetUserByIdQuery : AbstractQuery<GetUserByIdQuery.User>
    {
        public string UserIdToGet { get; }

        public GetUserByIdQuery(string correlationId, string tenantId, string userId, string userIdToGet)
            : base(correlationId, tenantId, userId)
        {
            UserIdToGet = userIdToGet;
        }

        public class User
        {
            public string UserId { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }

            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }

            public IEnumerable<Role> Roles { get; set; } = new List<Role>();
        }

        public class Role
        {
            public string RoleId { get; set; }
            public string Name { get; set; }
        }
    }
}
