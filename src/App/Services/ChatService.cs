using App.Hubs;
using Cayent.Core.Common;
using Data.App.DbContext;
using Data.App.Models.Chats;
using Data.Common;
using Data.Identity.DbContext;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Chats;
using Cayent.Core.Common.Extensions;

namespace App.Services
{
    public class ChatService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHubContext<ChatHub, IChatClient> _hubContext;

        public ChatService(AppDbContext dbContext, IHubContext<ChatHub, IChatClient> hubContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        public async Task<Paged<SearchChatInfo>> SearchChat(string userId, string criteria, int pageIndex, int pageSize)
        {
            var sql = from chat in _dbContext.Chats.Include(p => p.Receivers).ThenInclude(p => p.Receiver)
                      join rcv in _dbContext.ChatReceivers on chat.ChatId equals rcv.ChatId
                      join msg in _dbContext.ChatMessages on chat.ChatId equals msg.ChatId

                      join lastMsg in _dbContext.ChatMessages on chat.LastChatMessageId equals lastMsg.ChatMessageId
                      join lastSndr in _dbContext.Users on lastMsg.SenderId equals lastSndr.UserId
                      //join lastAcnt in _dbContext.Accounts on lastSndr.UserId equals lastAcnt.AccountId

                      where rcv.ReceiverId == userId && !rcv.IsRemoved
                      where criteria == null || EF.Functions.Like(msg.Content, $"%{criteria}%")

                      select new SearchChatInfo
                      {
                          ChatId = chat.ChatId,
                          Title = "not implementedxx",//chat.Title(userId, "You"),
                          LastMessageText = lastMsg.Content,
                          IsRead = rcv.LastChatMessageId == chat.LastChatMessageId,
                          LastDateSent = lastMsg.DateSent,
                          SenderFirstLastName = lastSndr.UserId == userId ? "You" : lastSndr.FirstLastName,
                          SenderInitials = lastSndr.Initials,
                          SenderProfilePicture32 = lastSndr.ImageId
                      }
                        ;
            var paged = await sql.ToPagedItemsAsync(pageIndex, pageSize);

            //var infos = paged.Select(p => new SearchChatInfo
            //{
            //    ChatId = p.ChatId,
            //    Title = p.Title,
            //    LastMessageText = p.LastMessageText,
            //    IsRead = p.IsRead,
            //    LastDateSent = p.LastDateSent,
            //    SenderFirstLastName = p.SenderFirstLastName,
            //    SenderInitials = p.SenderInitials,
            //    SenderProfilePicture32 = p.SenderProfilePicture32
            //});

            //var pagedItems = await query
            //    .AsNoTracking()
            //    .ToPagedItemsAsync(pageIndex, pageSize);

            //var paginated = new Paginated<SearchChatInfo>(infos, paged.TotalCount, paged.PageIndex, paged.PageSize);

            return await Task.FromResult(paged);
        }

        public async Task<Paged<SearchChatInfo>> SearchUnreadChats(string userId, string criteria, int pageIndex, int pageSize)
        {
            var query = from cr in _dbContext.ChatReceivers
                        join c in _dbContext.Chats on cr.ChatId equals c.ChatId
                        join msg in _dbContext.ChatMessages on c.ChatId equals msg.ChatId
                        join sender in _dbContext.Users on msg.SenderId equals sender.UserId
                        //join accnt in _dbContext.Accounts on sender.Id equals accnt.AccountId
                        where cr.ReceiverId == userId && !cr.IsRemoved
                        where cr.LastChatMessageId != c.LastChatMessageId
                        select new SearchChatInfo
                        {
                            ChatId = c.ChatId,
                            LastMessageText = msg.Content,
                            IsRead = false,
                            LastDateSent = msg.DateSent,
                            SenderFirstLastName = sender.UserId == userId ? "You" : sender.FirstLastName,
                            SenderInitials = sender.Initials,
                            SenderProfilePicture32 = sender.ImageId
                        };

            //var paged = await query
            //    .AsNoTracking()
            //    .ToPagedItemsAsync(pageIndex, pageSize);

            //var paginated = new Paged<SearchChatInfo>(paged, paged.TotalCount, paged.PageIndex, paged.PageSize);
            throw new NotImplementedException();
            //return await Task.FromResult(query.ToPagedItemsAsync(pageIndex, pageSize));
        }

        public async Task<Paged<ChatMessageInfo>> SearchChatMessage(string userId, string chatId, string criteria, int pageIndex = 1, int pageSize = 10)
        {
            var paged = await _dbContext
                .ChatMessages
                .AsNoTracking()
                .Include(p => p.Chat)
                .Include(p => p.Sender)
                //.ThenInclude(p => p.User)
                .Where(p => p.Chat.Receivers.Any(q => q.ReceiverId == userId && !q.IsRemoved))
                .Where(p => p.ChatId == chatId)
                //.Where(p => string.IsNullOrWhiteSpace(criteria)
                //    || p.Content.Contains(criteria, StringComparison.InvariantCulture))
                .ToPagedItemsAsync(pageIndex, pageSize);

            //var infos = paged.Select(p =>
            //{
            //    var info = new ChatMessageInfo
            //    {
            //        ChatMessageId = p.ChatMessageId,
            //        //ChatId = p.ChatId,
            //        Content = p.Content,
            //        DateSent = p.DateSent,
            //        Sender = new ChatMessageInfo.SenderInfo
            //        {
            //            UserId = p.Sender.UserId,
            //            FirstName = p.Sender.FirstName,
            //            LastName = p.Sender.LastName,
            //            Initials = p.Sender.Initials,
            //            ProfilePicture32 = p.Sender.Account.ProfilePicture32
            //        }
            //    };

            //    return info;
            //});

            //var paginated = new Paginated<ChatMessageInfo>(infos, paged.TotalCount, paged.PageIndex, paged.PageSize);

            throw new NotImplementedException();

            //return await Task.FromResult(paged);
        }

        public async Task<ChatDetailInfo> GetChatDetail(string userId, string chatId)
        {
            var data = await _dbContext
                .Chats
                .AsNoTracking()
                .Include(p => p.Messages)
                .Include(p => p.Receivers)
                    .ThenInclude(p => p.Receiver)
                //.ThenInclude(p => p.User)

                .FirstOrDefaultAsync(p => p.ChatId == chatId && p.Receivers.Any(q => q.ReceiverId == userId));

            var model = new ChatDetailInfo();
            if (data != null)
            {
                model = new ChatDetailInfo
                {
                    ChatId = data.ChatId,
                    NumberOfMessages = data.Messages.Count,
                    Title = data.Title, //data.Title(userId, "You"),
                    HasPendingMessage = data.Receivers.Any(p => p.ReceiverId == userId && p.LastChatMessageId != data.LastChatMessageId),
                    Receivers = data.Receivers.Select(p => new ChatDetailInfo.ChatReceiverInfo
                    {
                        UserId = p.Receiver.UserId,
                        FirstName = p.Receiver.FirstName,
                        LastName = p.Receiver.LastName,
                        Initials = p.Receiver.Initials,
                        ProfilePicture32 = p.Receiver.ImageId,
                        IsRemoved = p.IsRemoved
                    }).ToList()
                };
            }

            return model;
        }

        public async Task<string> CreateChat(string memberId1, string memberId2)
        {
            //  check if already existing
            var exists = await _dbContext.Chats
                .Include(p => p.Receivers)
                .Where(p => p.Receivers.Count == 2
                        && p.Receivers.Any(q => q.ReceiverId == memberId1)
                        && p.Receivers.Any(q => q.ReceiverId == memberId2))
                .FirstOrDefaultAsync();

            if (exists != null)
            {
                //  reactivate member2
                var member = exists.Receivers.First(p => p.ReceiverId == memberId2);

                if (member.IsRemoved)
                {
                    member.IsRemoved = false;

                    await _dbContext.SaveChangesAsync();
                    await AddChatMessage(exists.ChatId, memberId2, "Joined the Chat.", EnumChatMessageType.System);
                }
                return await Task.FromResult(exists.ChatId);
            }

            var chat = new Chat
            {
                ChatId = Guid.NewGuid().ToString(),
                LastChatMessageId = string.Empty
            };

            var members = new[] { memberId1, memberId2 };

            var chatReceivers = members.Select(p =>
            {
                var info = new ChatReceiver
                {
                    ChatReceiverId = Guid.NewGuid().ToString(),
                    ChatId = chat.ChatId,
                    ReceiverId = p,
                    IsRemoved = false
                };

                return info;
            });

            //  chat
            await _dbContext.AddAsync(chat);
            //  receivers
            await _dbContext.AddRangeAsync(chatReceivers);
            //  save
            await _dbContext.SaveChangesAsync();

            //  starting message
            await AddChatMessage(chat.ChatId, memberId1, "Chat Started.", EnumChatMessageType.System);

            return await Task.FromResult(chat.ChatId);
        }

        public async Task AddChatMember(string chatId, List<string> userIds)
        {
            var chatReceivers = userIds.Select(p =>
            {
                var info = new ChatReceiver
                {
                    ChatReceiverId = Guid.NewGuid().ToString(),
                    ChatId = chatId,
                    ReceiverId = p,
                    IsRemoved = false
                };

                return info;
            });

            await _dbContext.AddAsync(chatReceivers);
            await _dbContext.SaveChangesAsync();

            await Task.CompletedTask;
        }

        public async Task<string> AddChatMessage(string chatId, string senderId, string content, EnumChatMessageType chatMessageType)
        {
            var chat = await _dbContext
                .Chats
                .Include(p => p.Receivers)
                .Where(p => p.Receivers.Any(q => q.ReceiverId == senderId) || chatMessageType == EnumChatMessageType.System)
                .FirstOrDefaultAsync(p => p.ChatId == chatId);

            var now = DateTime.UtcNow;

            var chatMessage = new ChatMessage
            {
                ChatMessageId = Guid.NewGuid().ToString(),
                ChatId = chatId,
                Content = content,
                DateSent = now,
                SenderId = chatMessageType == EnumChatMessageType.System ? null : senderId,
                ChatMessageType = chatMessageType,
            };

            chat.LastChatMessageId = chatMessage.ChatMessageId;

            //  this sender has read this message
            chat.Receivers.First(p => p.ReceiverId == senderId).LastChatMessageId = chatMessage.ChatMessageId;

            await _dbContext.AddAsync(chatMessage);

            await _dbContext.SaveChangesAsync();

            #region Send notifications

            var sender = await _dbContext
                .Users
                .AsNoTracking()
                //Include(p => p.Account)
                .FirstAsync(q => q.UserId == senderId);

            var receivedMessageInfo = new ChatMessageReceivedInfo
            {
                ChatId = chat.ChatId,
                ChatMessageId = chatMessage.ChatMessageId,
                ChatMessageType = (int)chatMessage.ChatMessageType,
                Content = chatMessage.Content,
                DateSent = chatMessage.DateSent,
                Sender = new ChatMessageReceivedInfo.SenderInfo
                {
                    UserId = sender.UserId,
                    Initials = sender.Initials,
                    FirstName = sender.FirstName,
                    LastName = sender.LastName,
                    ProfilePicture32 = sender.ImageId
                }
            };


            //  notify the other receivers, targets the navbar
            await _hubContext.Clients.Users(chat.Receivers.Select(p => p.ReceiverId).ToArray()).ChatMessageReceived(receivedMessageInfo);
            //  notify open chat clients
            await _hubContext.Clients.Groups(chat.ChatId).ChatMessageReceivedFromGroup(receivedMessageInfo);

            #endregion

            return await Task.FromResult(chatMessage.ChatMessageId);
        }

        public async Task<ChatMessageInfo> GetChatMessage(string chatMessageId)
        {
            var data = await _dbContext.ChatMessages
                .AsNoTracking()
                .Include(p => p.Sender)
                //.ThenInclude(p => p.User)
                .FirstOrDefaultAsync(p => p.ChatMessageId == chatMessageId);

            if (data != null)
            {
                return new ChatMessageInfo
                {
                    ChatMessageId = data.ChatMessageId,
                    //ChatId = data.ChatId,
                    Content = data.Content,
                    DateSent = data.DateSent,
                    Sender = new ChatMessageInfo.SenderInfo
                    {
                        UserId = data.Sender.UserId,
                        FirstName = data.Sender.FirstName,
                        LastName = data.Sender.LastName,
                        ProfilePicture32 = data.Sender.ImageId,
                        Initials = data.Sender.Initials
                    }
                };
            }
            return null;
        }

        public async Task MarkChatAsRead(string userId, string chatId)
        {
            var chat = await _dbContext
                .Chats
                .Include(p => p.Receivers)
                .FirstOrDefaultAsync(p => p.ChatId == chatId);

            if (chat != null && !string.IsNullOrWhiteSpace(chat.LastChatMessageId))
            {
                var receiver = chat.Receivers.FirstOrDefault(p => p.ReceiverId == userId);

                if (receiver != null)
                {
                    receiver.LastChatMessageId = chat.LastChatMessageId;

                    await _dbContext.SaveChangesAsync();
                }
            }

            //await Task.CompletedTask;
        }

        public async Task RemoveChatMember(string chatId, string memberId)
        {
            var chat = await _dbContext
                .Chats
                .Include(p => p.Receivers)
                    .ThenInclude(p => p.Receiver)
                .Include(e => e.Messages)
                .Where(p => p.ChatId == chatId && p.Receivers.Any(q => q.ReceiverId == memberId))
                .FirstOrDefaultAsync();

            if (chat != null)
            {
                var cr = chat.Receivers.First(p => p.ReceiverId == memberId);
                cr.IsRemoved = true;

                if (chat.Receivers.Count(p => !p.IsRemoved) > 0)
                {
                    //  notify leaving
                    await AddChatMessage(chat.ChatId, memberId, $"{cr.Receiver.FirstLastName} has left the chat.", EnumChatMessageType.System);
                }

                if (chat.Receivers.Count(p => !p.IsRemoved) == 0)
                {
                    //  notify to close any connected client
                    await _hubContext.Clients.Group(chat.ChatId).ChatDeleted(chat.ChatId);

                    _dbContext.RemoveRange(chat.Receivers);
                    _dbContext.RemoveRange(chat.Messages);
                    _dbContext.Remove(chat);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }

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

}
