using App.CQRS.Chats.Common.Queries.Query;
using Cayent.Core.Common;
using Cayent.Core.CQRS.Queries;
using Data.App.DbContext;
using Data.Common;
using Data.Identity.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cayent.Core.Common.Extensions;

namespace App.CQRS.Chats.Common.Queries.Handler
{
    public sealed class ChatQueryHandler :
        IQueryHandler<GetChatHeaderByIdQuery, GetChatHeaderByIdQuery.ChatHeader>,
        IQueryHandler<SearchChatMessagesQuery, Paged<SearchChatMessagesQuery.ChatMessage>>,
        IQueryHandler<SearchChatQuery, Paged<SearchChatQuery.Chat>>,
        IQueryHandler<GetChatByMemberIdQuery, GetChatByMemberIdQuery.Chat>

    {
        private readonly AppDbContext _dbContext;
        public ChatQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        async Task<GetChatHeaderByIdQuery.ChatHeader> IQueryHandler<GetChatHeaderByIdQuery, GetChatHeaderByIdQuery.ChatHeader>.HandleAsync(GetChatHeaderByIdQuery query, System.Threading.CancellationToken cancellationToken)
        {
            var sql1 = from chat in _dbContext.Chats

                       where chat.ChatId == query.ChatId

                       select new GetChatHeaderByIdQuery.ChatHeader
                       {
                           ChatId = chat.ChatId,
                           Title = "not implementedxx",//chat.Title(query.Id, query.ReplaceWith),
                           NumberOfMessages = chat.Messages.Count,
                           HasPendingMessage = chat.Receivers.Any(p => 
                               !string.IsNullOrWhiteSpace(chat.LastChatMessageId) 
                               && p.ReceiverId == query.UserId 
                               && p.LastChatMessageId != chat.LastChatMessageId
                               )
                       };


            var dto = await sql1.FirstOrDefaultAsync();

            if (dto != null)
            {
                var receivers = from rcv in _dbContext.ChatReceivers

                                where rcv.ChatId == dto.ChatId

                                select new GetChatHeaderByIdQuery.Receiver
                                {
                                    UserId = rcv.Receiver.UserId,
                                    FirstName = rcv.Receiver.FirstName,
                                    LastName = rcv.Receiver.LastName,
                                    Initials = rcv.Receiver.Initials,
                                    IsRemoved = rcv.IsRemoved,
                                    ProfilePicture32 = rcv.Receiver.Image.Url
                                };

                dto.Receivers = await receivers.ToListAsync();
            }


            return dto;
        }

        async Task<Paged<SearchChatMessagesQuery.ChatMessage>> IQueryHandler<SearchChatMessagesQuery, Paged<SearchChatMessagesQuery.ChatMessage>>.HandleAsync(SearchChatMessagesQuery query, System.Threading.CancellationToken cancellationToken)
        {
            var sql1 = from chatMsg in _dbContext.ChatMessages

                       where chatMsg.ChatId == query.ChatId

                       orderby chatMsg.DateSent ascending

                       select new SearchChatMessagesQuery.ChatMessage
                       {
                           ChatMessageId = chatMsg.ChatMessageId,
                           Content = chatMsg.Content,
                           DateSent = chatMsg.DateSent,
                           ChatMessageType = chatMsg.ChatMessageType,
                           Sender = new SearchChatMessagesQuery.Sender
                           {
                               UserId = chatMsg.Sender.UserId,
                               FirstName = chatMsg.Sender.FirstName,
                               LastName = chatMsg.Sender.LastName,
                               Initials = chatMsg.Sender.Initials,
                               ProfilePicture32 = chatMsg.Sender.ImageId
                           }
                       };

            var dto = await sql1.ToPagedItemsAsync(query.PageIndex, query.PageSize);

            return dto;

        }

        async Task<Paged<SearchChatQuery.Chat>> IQueryHandler<SearchChatQuery, Paged<SearchChatQuery.Chat>>.HandleAsync(SearchChatQuery query, System.Threading.CancellationToken cancellationToken)
        {
            var sql = from c in _dbContext.Chats

                      join cm in _dbContext.ChatMessages on c.LastChatMessageId equals cm.ChatMessageId// into cm1
                                                                                                       //from cm in cm1.DefaultIfEmpty()

                      join cr in _dbContext.ChatReceivers on c.ChatId equals cr.ChatId// into cr1
                                                                                      //from cr in cr1.DefaultIfEmpty()

                      //join acc in _dbContext.Accounts on cm.SenderId equals acc.AccountId
                      join usr in _dbContext.Users on cm.SenderId equals usr.UserId

                      where cr.ReceiverId == query.UserId
                      where _dbContext.ChatReceivers.Any(e => e.ChatId == c.ChatId && e.ReceiverId == query.UserId)

                      select new SearchChatQuery.Chat
                      {
                          ChatId = c.ChatId,
                          Title = c.Title,
                          LastMessageText = cm.Content,
                          IsRead = cr.LastChatMessageId == c.LastChatMessageId,
                          LastDateSent = cm.DateSent,
                          SenderFirstName = usr.UserId == query.UserId ? query.ReplaceWith : usr.LastName,
                          SenderLastName = usr.UserId == query.UserId ? "" : usr.LastName,
                          SenderInitials = usr.Initials,
                          SenderProfilePicture32 = usr.ImageId
                      };

            switch (query.SortField?.ToLower())
            {
                case "sender":
                    sql = query.SortOrder == 1 ? sql.OrderBy(e => e.SenderFirstName).ThenBy(e => e.SenderLastName)
                        : sql.OrderByDescending(e => e.SenderFirstName).ThenByDescending(e => e.SenderLastName);
                    break;
                case "title":
                    sql = query.SortOrder == 1 ? sql.OrderBy(e => e.Title) : sql.OrderByDescending(e => e.Title);
                    break;
                case "message":
                    sql = query.SortOrder == 1 ? sql.OrderBy(e => e.LastMessageText) : sql.OrderByDescending(e => e.LastMessageText);
                    break;
                case "date":
                    sql = query.SortOrder == 1 ? sql.OrderBy(e => e.LastDateSent) : sql.OrderByDescending(e => e.LastDateSent);
                    break;
                default:
                    sql = sql.OrderBy(e => e.SenderFirstName).ThenBy(e => e.SenderLastName);
                    break;
            }

            var dto = await sql.ToPagedItemsAsync(query.PageIndex, query.PageSize);

            return dto;
        }

        async Task<GetChatByMemberIdQuery.Chat> IQueryHandler<GetChatByMemberIdQuery, GetChatByMemberIdQuery.Chat>.HandleAsync(GetChatByMemberIdQuery query, System.Threading.CancellationToken cancellationToken)
        {
            var data = await _dbContext.Chats
                        .Include(e => e.Receivers)
                        .AsNoTracking()
                        .Where(e => e.Receivers.Count == 2 && e.Receivers.Any(p => p.ReceiverId == query.MemberId1) && e.Receivers.Any(p => p.ReceiverId == query.MemberId2))
                        .FirstOrDefaultAsync();

            if (data == null)
                return null;

            return new GetChatByMemberIdQuery.Chat
            {
                ChatId = data.ChatId,
                Title = data.Title
            };
        }
    }
}
