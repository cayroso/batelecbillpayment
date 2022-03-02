using App.CQRS.Chats.Common.Commands.Command;
using App.Hubs;
using Data.App.Models.Chats;
using Data.Identity.DbContext;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.App.DbContext;
using Cayent.Core.CQRS.Commands;
using ViewModel.Chats;

namespace App.CQRS.Chats.Common.Commands.Handler
{
    public sealed class ChatCommandHandler :
        ICommandHandler<AddChatCommand>,
        ICommandHandler<AddChatMessageCommand>
    {
        private readonly AppDbContext _dbContext;
        private readonly IHubContext<ChatHub, IChatClient> _hubContext;

        public ChatCommandHandler(AppDbContext dbContext, IHubContext<ChatHub, IChatClient> hubContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        async Task ICommandHandler<AddChatCommand>.HandleAsync(AddChatCommand command, System.Threading.CancellationToken cancellationToken)
        {
            //  check if already existing
            var exists = await _dbContext.Chats
                .Include(p => p.Receivers)
                .Where(p => p.Receivers.Count == 2
                        && p.Receivers.Any(q => q.ReceiverId == command.MemberId1)
                        && p.Receivers.Any(q => q.ReceiverId == command.MemberId2))
                .FirstOrDefaultAsync();

            if (exists != null)
            {
                //  reactivate member2
                var member = exists.Receivers.First(p => p.ReceiverId == command.MemberId2);

                if (member.IsRemoved)
                {
                    member.IsRemoved = false;

                    await _dbContext.SaveChangesAsync();

                    var cmd1 = new AddChatMessageCommand(command.CorrelationId, command.TenantId, command.UserId, exists.ChatId, command.MemberId2, "Rejoining the chat.", EnumChatMessageType.System);

                    //TODO: fix next line
                    //await HandleAsync(cmd1, cancellationToken);
                }

                return;
            }

            var members = new[] { command.MemberId1, command.MemberId2 };

            var chat = new Chat
            {
                ChatId = command.ChatId,
                Title = await CreateChatTitle(members),
                LastChatMessageId = string.Empty
            };

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
            var cmd2 = new AddChatMessageCommand(command.CorrelationId, command.TenantId, command.UserId, command.ChatId, command.MemberId1, "Joined the chat.", EnumChatMessageType.System);

            //TODO: fix next line
            //await HandleAsync(cmd2);
        }

        async Task ICommandHandler<AddChatMessageCommand>.HandleAsync(AddChatMessageCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var chat = await _dbContext
                .Chats
                .Include(p => p.Receivers)
                .Where(p => p.Receivers.Any(q => q.ReceiverId == command.SenderId))
                .FirstOrDefaultAsync(p => p.ChatId == command.ChatId);

            var now = DateTime.UtcNow;

            var chatMessage = new ChatMessage
            {
                ChatMessageId = Guid.NewGuid().ToString(),
                ChatId = command.ChatId,
                Content = command.Content,
                DateSent = now,
                SenderId = command.SenderId,
                ChatMessageType = command.ChatMessageType,
            };

            chat.LastChatMessageId = chatMessage.ChatMessageId;

            //  this sender has read this message
            chat.Receivers.First(p => p.ReceiverId == command.SenderId).LastChatMessageId = chatMessage.ChatMessageId;

            await _dbContext.AddAsync(chatMessage);

            await _dbContext.SaveChangesAsync();

            #region Send notifications

            var sender = await _dbContext
                .Users
                .AsNoTracking()
                .FirstAsync(q => q.UserId == command.SenderId);

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
                    Initials = $"{sender.FirstName[0]} {sender.LastName[0]}",
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
        }

        async Task<string> CreateChatTitle(string[] memberIds)
        {
            var users = await _dbContext.Users
                        //.Include(e => e.Account)
                        .Where(e => memberIds.Contains(e.UserId))
                        .ToListAsync();


            var names = new List<string>();

            users.ForEach(e =>
            {
                var entry = e.FirstName;// $"{e.Id}/{e.Account.FirstName}";

                names.Add(entry);
            });

            var title = string.Join(", ", names.ToArray());

            return title;
        }
    }
}
