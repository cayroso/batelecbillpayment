using Cayent.Core.Common.Extensions;
using Data.App.Models.Chats;
using Data.Common;
using System;
using Cayent.Core.CQRS.Queries;
using Cayent.Core.Common;

namespace App.CQRS.Chats.Common.Queries.Query
{
    public sealed class SearchChatMessagesQuery : AbstractQuery<Paged<SearchChatMessagesQuery.ChatMessage>>
    {
        public SearchChatMessagesQuery(string correlationId, string tenantId, string userId, string chatId, string criteria, int pageIndex, int pageSize)
            : base(correlationId, tenantId, userId)
        {
            ChatId = chatId;
            Criteria = criteria;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string ChatId { get; }
        public string Criteria { get; }
        public int PageIndex { get; }
        public int PageSize { get; }

        public class ChatMessage
        {
            public string ChatMessageId { get; set; }
            public EnumChatMessageType ChatMessageType { get; set; }
            public Sender Sender { get; set; }
            public string Content { get; set; }

            DateTime _dateSent;
            public DateTime DateSent
            {
                get => _dateSent;
                set => _dateSent = value.AsUtc();
            }

            //public List<Receiver> Receivers { get; set; } = new List<Receiver>();
        }

        public class Sender
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Initials { get; set; }
            public string ProfilePicture32 { get; set; }
        }
    }
}
