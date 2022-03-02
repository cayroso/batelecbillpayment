using Cayent.Core.CQRS.Queries;
using Cayent.Core.Common.Extensions;
using Data.Common;
using System;
using Cayent.Core.Common;

namespace App.CQRS.Navbar.Common.Queries.Query
{
    public sealed class GetUnreadChatsQuery : AbstractQuery<Paged<GetUnreadChatsQuery.ChatMessage>>
    {
        public GetUnreadChatsQuery(string correlationId, string tenantId, string userId, int pageIndex, int pageSize)
            : base(correlationId, tenantId, userId)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int PageIndex { get; }
        public int PageSize { get; }

        public class ChatMessage
        {
            public string ChatId { get; set; }
            public string Title { get; set; }
            public string LastMessageText { get; set; }

            private DateTime _lastDateSent { get; set; }
            public DateTime LastDateSent {
                get => _lastDateSent;
                set => _lastDateSent = value.AsUtc();

            }
            public string SenderFirstLastName { get; set; }
            public string SenderInitials { get; set; }
            public string SenderProfilePicture32 { get; set; }
            public bool IsRead { get; set; }
        }
    }
}
