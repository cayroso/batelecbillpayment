using Cayent.Core.CQRS.Commands;
using Data.App.Models.Chats;

namespace App.CQRS.Chats.Common.Commands.Command
{
    public sealed class AddChatMessageCommand : AbstractCommand
    {
        public AddChatMessageCommand(string correlationId, string tenantId, string userId, string chatId, string senderId, string content, EnumChatMessageType chatMessageType)
            : base(correlationId, tenantId, userId)
        {
            ChatId = chatId;
            SenderId = senderId;
            Content = content;
            ChatMessageType = chatMessageType;
        }

        public string ChatId { get; }
        public string SenderId { get; }
        public string Content { get; }
        public EnumChatMessageType ChatMessageType { get; }
    }
}
