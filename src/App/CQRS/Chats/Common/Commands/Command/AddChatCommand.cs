using Cayent.Core.CQRS.Commands;

namespace App.CQRS.Chats.Common.Commands.Command
{
    public sealed class AddChatCommand: AbstractCommand
    {
        public AddChatCommand(string correlationId, string tenantId, string userId, string chatId, string memberId1, string memberId2)
            :base(correlationId, tenantId, userId)
        {
            ChatId = chatId;
            MemberId1 = memberId1;
            MemberId2 = memberId2;
        }

        public string ChatId { get; }
        public string MemberId1 { get; }
        public string MemberId2 { get; }
    }
}
