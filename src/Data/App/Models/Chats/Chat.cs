using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Chats
{
    public class Chat
    {
        public string ChatId { get; set; }

        public string LastChatMessageId { get; set; }

        public string Title { get; set; }

        public virtual ICollection<ChatReceiver> Receivers { get; set; } = new List<ChatReceiver>();
        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
}
