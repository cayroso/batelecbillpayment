using App.CQRS.Navbar.Common.Queries.Query;
using Cayent.Core.Common;
using Cayent.Core.CQRS.Queries;
using Data.App.DbContext;
using Data.Common;
using Data.Identity.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cayent.Core.Common.Extensions;

namespace App.CQRS.Navbar.Common.Queries.Handler
{
    public sealed class NavbarQueryHandler :
        IQueryHandler<GetUnreadChatsQuery, Paged<GetUnreadChatsQuery.ChatMessage>>,
        IQueryHandler<GetUnreadNotificationsQuery, Paged<GetUnreadNotificationsQuery.Notification>>
    {
        private readonly AppDbContext _dbContext;
        public NavbarQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        async Task<Paged<GetUnreadChatsQuery.ChatMessage>> IQueryHandler<GetUnreadChatsQuery, Paged<GetUnreadChatsQuery.ChatMessage>>.HandleAsync(GetUnreadChatsQuery query, System.Threading.CancellationToken cancellationToken)
        {
            var sql = from cr in _dbContext.ChatReceivers
                      join c in _dbContext.Chats on cr.ChatId equals c.ChatId
                      join msg in _dbContext.ChatMessages on c.ChatId equals msg.ChatId //into msgs
                      //from msg in msgs.Where(e => e.ChatMessageId == c.LastChatMessageId).DefaultIfEmpty()


                      join sender in _dbContext.Users on msg.SenderId equals sender.UserId
                      //join accnt in _dbContext.Accounts on sender.Id equals accnt.AccountId

                      where cr.ReceiverId == query.UserId && !cr.IsRemoved
                      where msg.ChatMessageId == c.LastChatMessageId
                      where cr.LastChatMessageId != c.LastChatMessageId || cr.LastChatMessageId == null

                      orderby msg.DateSent ascending

                      select new GetUnreadChatsQuery.ChatMessage
                      {
                          ChatId = c.ChatId,
                          LastMessageText = msg.Content,
                          IsRead = false,
                          LastDateSent = msg.DateSent,
                          SenderFirstLastName = sender.UserId == query.UserId ? "You" : sender.FirstLastName,
                          SenderInitials = sender.Initials,
                          SenderProfilePicture32 = sender.ImageId
                      };

            var dto = await sql.ToPagedItemsAsync(query.PageIndex, query.PageSize);

            return dto;
        }

        async Task<Paged<GetUnreadNotificationsQuery.Notification>> IQueryHandler<GetUnreadNotificationsQuery, Paged<GetUnreadNotificationsQuery.Notification>>.HandleAsync(GetUnreadNotificationsQuery query, System.Threading.CancellationToken cancellationToken)
        {
            //var sql = from nr in _dbContext.NotificationReceivers
            //          join n in _dbContext.Notifications on nr.NotificationId equals n.NotificationId

            //          where nr.ReceiverId == query.UserId && DateTime.UtcNow <= nr.DateRead// nr.IsRead == false

            //          orderby n.DateSent descending

            //          select new GetUnreadNotificationsQuery.Notification
            //          {
            //              NotificationId = n.NotificationId,
            //              NotificationType = (int)n.NotificationType,
            //              Content = n.Content,
            //              DateSent = n.DateSent,
            //              IconClass = n.IconClass,
            //              ReferenceId = n.ReferenceId,
            //              Subject = n.Subject,
            //              DateRead = nr.DateRead,
            //              DateReceived = nr.DateReceived
            //          };

            //var dto = await sql.ToPagedItemsAsync(query.PageIndex, query.PageSize);

            //return dto;
            throw new NotImplementedException();
        }
    }
}
