using App.Hubs;
using Data.Identity.DbContext;
using Data.Identity.Models.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public class NotificationService
    {
        private readonly IdentityWebContext _dbContext;
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

        public NotificationService(IdentityWebContext dbContext, IHubContext<NotificationHub, INotificationClient> hubContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        public async Task AddNotification(string referenceId, string iconClass, string subject, string content, EnumNotificationType notificationType, EnumNotificationEntityClass notificationEntityClass, string[] userIds, string[] roleNames, CancellationToken cancellationToken, bool saveOnly = false)
        {
            if ((userIds == null || !userIds.Any()) && (roleNames == null || !roleNames.Any()))
            {
                await Task.CompletedTask;
            }

            var notificationId = Guid.NewGuid().ToString();

            var notification = new Notification
            {
                NotificationId = notificationId,
                IconClass = iconClass,
                Subject = subject,
                Content = content,
                ReferenceId = referenceId,
                DateSent = DateTime.UtcNow, 
                NotificationType = notificationType,
                NotificationEntityClass = notificationEntityClass
            };

            var combinedUserIds = new List<string>();

            if (userIds != null && userIds.Any())
            {
                foreach (var userId in userIds)
                {
                    notification.Receivers.Add(new NotificationReceiver
                    {
                        NotificationId = notification.NotificationId,
                        ReceiverId = userId,
                        DateRead = DateTime.MaxValue,
                        DateReceived = notification.DateSent
                    });

                    combinedUserIds.Add(userId);
                }
            }

            if (roleNames != null && roleNames.Any())
            {
                foreach (var roleName in roleNames)
                {
                    //var urs = await _dbContext
                    //    .UserRoles
                    //    .Include(p => p.Role)
                    //    .Where(p => p.Role.Name == roleName)
                    //    .ToListAsync();

                    //urs.ForEach(p =>
                    //{
                    //    notification.Receivers.Add(new NotificationReceiver
                    //    {
                    //        NotificationId = notification.NotificationId,
                    //        ReceiverId = p.UserId,
                    //        DateRead = DateTimeOffset.MaxValue,
                    //        DateReceived = notification.DateSent
                    //    });

                    //    combinedUserIds.Add(p.UserId);
                    //});
                }
            }

            await _dbContext.AddAsync(notification, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            if (!saveOnly && combinedUserIds.Any())
                await _hubContext.Clients.Users(combinedUserIds).Received(notification);

            await Task.CompletedTask;

        }

        //public async Task JobUpdated(string id, string[] userIds)
        //{
        //    await _hubContext.Clients.Users(userIds).JobUpdated(id);
        //}

        //public async Task<Paged<SearchNotificationInfo>> GetNotificationsAsync(string userId, bool onlyUnread, string criteria, int pageIndex = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        //{
        //    var query = from nr in _dbContext.NotificationReceivers
        //                join n in _dbContext.Notifications on nr.NotificationId equals n.NotificationId
        //                where nr.ReceiverId == userId && (onlyUnread == false || DateTime.UtcNow <= nr.DateRead)//nr.IsRead == false)
        //                where criteria == null || EF.Functions.Like(n.Subject, $"%{criteria}%") || EF.Functions.Like(n.Content, $"%{criteria}%")
        //                select new SearchNotificationInfo
        //                {
        //                    NotificationId = n.NotificationId,
        //                    NotificationType = (int)n.NotificationType,
        //                    Content = n.Content,
        //                    DateSent = n.DateSent,
        //                    IconClass = n.IconClass,
        //                    ReferenceId = n.ReferenceId,
        //                    Subject = n.Subject,
        //                    DateRead = nr.DateRead,
        //                    DateReceived = nr.DateReceived
        //                };

        //    var pagedItems = await query
        //        .AsNoTracking()
        //        .ToPagedItemsAsync(pageIndex, pageSize, cancellationToken);

        //    pagedItems.Items.ToList().ForEach(item =>
        //    {
        //        item.DateRead = DateTime.SpecifyKind(item.DateRead, DateTimeKind.Utc);
        //        item.DateReceived = DateTime.SpecifyKind(item.DateReceived, DateTimeKind.Utc);
        //        item.DateSent = DateTime.SpecifyKind(item.DateSent, DateTimeKind.Utc);
        //    });
        //    //var paginated = new Paginated<SearchNotificationInfo>(pagedItems, pagedItems.TotalCount, pagedItems.PageIndex, pagedItems.PageSize);

        //    return await Task.FromResult(pagedItems);
        //}

        //public async Task<SearchNotificationInfo> GetNotificationAsync(string userId, string notificationId, CancellationToken cancellationToken)
        //{
        //    var query = from nr in _dbContext.NotificationReceivers
        //                join n in _dbContext.Notifications on nr.NotificationId equals n.NotificationId
        //                where n.NotificationId == notificationId && nr.ReceiverId == userId
        //                select new SearchNotificationInfo
        //                {
        //                    NotificationId = n.NotificationId,
        //                    Content = n.Content,
        //                    DateRead = nr.DateRead,
        //                    DateReceived = nr.DateReceived,
        //                    DateSent = n.DateSent,
        //                    IconClass = n.IconClass,
        //                    NotificationType = (int)n.NotificationType,
        //                    ReferenceId = n.ReferenceId,
        //                    Subject = n.Subject
        //                };

        //    var data = await query
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(cancellationToken);

        //    if (data != null)
        //    {
        //        data.DateRead = DateTime.SpecifyKind(data.DateRead, DateTimeKind.Utc);
        //        data.DateReceived = DateTime.SpecifyKind(data.DateReceived, DateTimeKind.Utc);
        //        data.DateSent = DateTime.SpecifyKind(data.DateSent, DateTimeKind.Utc);

        //        return data;
        //    }

        //    return null;
        //}

        public async Task MarkNotificationAsRead(string userId, string notificationId, CancellationToken cancellationToken)
        {
            var data = await _dbContext
                .NotificationReceivers
                .Include(p => p.Notification)
                .Where(p => p.NotificationId == notificationId)
                .Where(p => p.ReceiverId == userId)

                .FirstOrDefaultAsync(cancellationToken);

            if (data != null)
            {
                data.DateRead = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            await Task.CompletedTask;
        }

        public async Task DeleteNotification(string userId, string notificationId, CancellationToken cancellationToken)
        {
            var data = await _dbContext
                .Notifications
                .Include(p => p.Receivers)
                .Where(p => p.NotificationId == notificationId)
                .Where(p => p.Receivers.Any(q => q.ReceiverId == userId))
                .FirstOrDefaultAsync(cancellationToken);

            if (data != null)
            {
                var toRemove = data.Receivers.First(p => p.ReceiverId == userId);

                if (toRemove != null)
                {
                    data.Receivers.Remove(toRemove);
                    _dbContext.Remove(toRemove);
                }

                if (!data.Receivers.Any())
                {
                    _dbContext.Remove(data);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            await Task.CompletedTask;
        }

        public async Task DeleteNotificationByReferenceId(string referenceId, CancellationToken cancellationToken)
        {
            var data = await _dbContext
                .Notifications
                .Include(e => e.Receivers)
                .Where(p => p.ReferenceId == referenceId)
                .FirstOrDefaultAsync(cancellationToken);

            if (data != null)
            {
                _dbContext.Remove(data);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteNotificationByReferenceId(string referenceId, string userId, CancellationToken cancellationToken)
        {
            var data = await _dbContext
                .Notifications
                .Include(p => p.Receivers)
                .Where(p => p.ReferenceId == referenceId)
                .Where(p => p.Receivers.Any(q => q.ReceiverId == userId))
                .FirstOrDefaultAsync(cancellationToken);

            if (data != null)
            {
                var toRemove = data.Receivers.First(p => p.ReceiverId == userId);

                if (toRemove != null)
                {
                    data.Receivers.Remove(toRemove);
                    _dbContext.Remove(toRemove);
                }

                if (!data.Receivers.Any())
                {
                    _dbContext.Remove(data);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
