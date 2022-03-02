using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.Chats
{
    public class SearchChatInfo
    {
        public string ChatId { get; set; }
        public string Title { get; set; }
        public string LastMessageText { get; set; }
        public DateTime LastDateSent { get; set; }
        public string SenderFirstLastName { get; set; }
        public string SenderInitials { get; set; }
        public string SenderProfilePicture32 { get; set; }
        public bool IsRead { get; set; }
    }

    public class ChatDetailInfo
    {
        public ChatDetailInfo()
        {
            Receivers = new List<ChatReceiverInfo>();
        }

        public string ChatId { get; set; }
        public string Title { get; set; }
        public int NumberOfMessages { get; set; }
        public bool HasPendingMessage { get; set; }
        public List<ChatReceiverInfo> Receivers { get; set; }

        public class ChatReceiverInfo
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Initials { get; set; }
            public string ProfilePicture32 { get; set; }
            public bool IsRemoved { get; set; }
        }

    }

    public class ChatMessageInfo
    {
        public string ChatMessageId { get; set; }
        //public string ChatId { get; set; }
        public int ChatMessageType { get; set; }
        public SenderInfo Sender { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }

        public class SenderInfo
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Initials { get; set; }
            public string ProfilePicture32 { get; set; }
        }
    }

    #region ChatHub Models

    public class ChatReceivedInfo
    {

    }

    public class ChatMessageReceivedInfo
    {
        public string ChatMessageId { get; set; }
        public string ChatId { get; set; }
        public int ChatMessageType { get; set; }
        public SenderInfo Sender { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }

        public class SenderInfo
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Initials { get; set; }
            public string ProfilePicture32 { get; set; }
        }
    }

    public class ChatReceiverAddedInfo
    {

    }

    public class ChatReceiverRemovedInfo
    {

    }

    #endregion
}
