using Cayent.Core.Common.Extensions;
using Data.App.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Chats
{
    public enum EnumChatMessageType
    {
        System = 0,
        User = 1
    }

    public class ChatMessage
    {
        public string ChatMessageId { get; set; }

        public string ChatId { get; set; }
        public virtual Chat Chat { get; set; }

        public string SenderId { get; set; }
        public virtual User Sender { get; set; }

        public string Content { get; set; }

        public EnumChatMessageType ChatMessageType { get; set; } = EnumChatMessageType.User;

        DateTime _dateSent;
        public DateTime DateSent
        {
            get => _dateSent;
            set => _dateSent = value.Truncate().AsUtc();
        }

        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<ChatReceiver> Receivers { get; set; }
    }
}
