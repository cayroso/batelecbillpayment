using Cayent.Core.CQRS.Queries;

namespace App.CQRS.Chats.Common.Queries.Query
{
    public sealed class GetChatByMemberIdQuery: AbstractQuery<GetChatByMemberIdQuery.Chat>
    {
        public GetChatByMemberIdQuery(string correlationId, string tenantId, string userId, string memberId1, string memberId2)
            :base(correlationId, tenantId, userId)
        {
            MemberId1 = memberId1;
            MemberId2 = memberId2;
        }

        public string MemberId1 { get; }
        public string MemberId2 { get; }

        public class Chat
        {
            public string ChatId { get; set; }
            public string Title { get; set; }
        }
    }
}
