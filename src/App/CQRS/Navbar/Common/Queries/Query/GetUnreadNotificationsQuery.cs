using Cayent.Core.CQRS.Queries;
using Cayent.Core.Common.Extensions;
using Data.Common;
using System;
using Cayent.Core.Common;

namespace App.CQRS.Navbar.Common.Queries.Query
{
    public sealed class GetUnreadNotificationsQuery: AbstractQuery<Paged<GetUnreadNotificationsQuery.Notification>>
    {
        public GetUnreadNotificationsQuery(string correlationId, string tenantId, string userId, int pageIndex, int pageSize)
            : base(correlationId, tenantId, userId)
        {            
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int PageIndex { get; }
        public int PageSize { get; }

        public class Notification
        {
            public string NotificationId { get; set; }
            public int NotificationType { get; set; }
            public string IconClass { get; set; }
            public string Subject { get; set; }
            public string Content { get; set; }
            public string ReferenceId { get; set; }
            
            DateTime _dateSent { get; set; }
            public DateTime DateSent { 
                get => _dateSent;
                set => _dateSent = value.AsUtc(); 
            }
            
            DateTime _dateReceived { get; set; }
            public DateTime DateReceived
            {
                get => _dateReceived;
                set => _dateReceived = value.AsUtc();
            }

            DateTime _dateRead { get; set; }
            public DateTime DateRead
            {
                get => _dateRead;
                set => _dateRead = value.AsUtc();
            }

            public bool IsRead => DateRead < DateTime.UtcNow;
        }
    }
}
