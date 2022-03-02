using Data.App.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Chats
{
    public class ChatReceiver
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ChatReceiverId { get; set; }

        public string ChatId { get; set; }
        public virtual Chat Chat { get; set; }

        public string ReceiverId { get; set; }
        public virtual User Receiver { get; set; }

        public string LastChatMessageId { get; set; }
        public virtual ChatMessage LastChatMessage { get; set; }

        public bool IsRemoved { get; set; }
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
}
