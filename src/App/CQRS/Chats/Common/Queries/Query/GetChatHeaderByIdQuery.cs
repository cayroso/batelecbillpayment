using System.Collections.Generic;
using Cayent.Core.CQRS.Queries;

namespace App.CQRS.Chats.Common.Queries.Query
{
    public sealed class GetChatHeaderByIdQuery : AbstractQuery<GetChatHeaderByIdQuery.ChatHeader>
    {
        public GetChatHeaderByIdQuery(string correlationId, string tenantId, string chatId, string userId, string replaceWith)
            : base(correlationId, tenantId, userId)
        {
            ChatId = chatId;
            ReplaceWith = replaceWith;
        }

        public string ChatId { get; }
        //public string UserId { get; }
        public string ReplaceWith { get; }

        public class ChatHeader
        {
            public string ChatId { get; set; }
            public string Title { get; set; }
            public int NumberOfMessages { get; set; }
            public bool HasPendingMessage { get; set; }
            public List<Receiver> Receivers { get; set; } = new List<Receiver>();
        }

        public class Receiver
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Initials { get; set; }
            public string ProfilePicture32 { get; set; }
            public bool IsRemoved { get; set; }
        }
    }
}
