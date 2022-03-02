using Cayent.Core.Common.Extensions;
using Data.Common;
using System;
using Cayent.Core.CQRS.Queries;
using Cayent.Core.Common;

namespace App.CQRS.Chats.Common.Queries.Query
{
    public sealed class SearchChatQuery : AbstractQuery<Paged<SearchChatQuery.Chat>>
    {
        public SearchChatQuery(string correlationId, string tenantId, string userId, string replaceWith, string criteria, int pageIndex, int pageSize, string sortField, int sortOrder)
            : base(correlationId, tenantId, userId)
        {
            ReplaceWith = replaceWith;
            Criteria = criteria;
            PageIndex = pageIndex;
            PageSize = pageSize;
            SortField = sortField;
            SortOrder = sortOrder;
        }

        //public string UserId { get; }
        public string ReplaceWith { get; }
        public string Criteria { get; }
        public int PageIndex { get; }
        public int PageSize { get; }
        public string SortField { get; }
        public int SortOrder { get; }

        public class Chat
        {
            public string ChatId { get; set; }
            public string SenderProfilePicture32 { get; set; }

            public string SenderFirstName { get; set; }
            public string SenderLastName { get; set; }
            public string SenderFirstLastName { get { return $"{SenderFirstName} {SenderLastName}".Trim(); } }
            public string SenderInitials { get; set; }
            public string Title { get; set; }
            public string LastMessageText { get; set; }

            private DateTime _lastDateSent;
            public DateTime LastDateSent
            {
                get => _lastDateSent;
                set => _lastDateSent = value.AsUtc();
            }

            public bool IsRead { get; set; }
        }
    }
}
