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

        #region Notification

        public async Task AddNotification(string referenceId, string iconClass, string subject, string content, DateTime dateSent, EnumNotificationType notificationType, EnumNotificationEntityClass notificationEntityClass, string[] userIds, string[] roleNames, CancellationToken cancellationToken, bool saveOnly = false)
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
                DateSent = dateSent,
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
                    var role = await _dbContext.Roles.FirstOrDefaultAsync(e => e.Name == roleName);

                    if (role == null)
                        continue;

                    var urs = await _dbContext
                        .UserRoles
                        .Where(p => p.RoleId == role.Id)
                        .ToListAsync();

                    urs.ForEach(p =>
                    {
                        notification.Receivers.Add(new NotificationReceiver
                        {
                            NotificationId = notification.NotificationId,
                            ReceiverId = p.UserId,
                            DateRead = DateTime.MaxValue,
                            DateReceived = notification.DateSent
                        });

                        combinedUserIds.Add(p.UserId);
                    });
                }
            }

            await _dbContext.AddAsync(notification, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            if (!saveOnly && combinedUserIds.Any())
            {
                var notifResponse = new NotificationResponse
                {
                    NotificationId = notification.NotificationId,
                    DateSent = notification.DateSent,
                    IconClass = notification.IconClass,
                    NotificationEntityClass = notification.NotificationEntityClass,
                    NotificationType = notification.NotificationType,
                    ReferenceId = notification.ReferenceId,
                    Subject = notification.Subject,
                    Content = notification.Content,
                };
                await _hubContext.Clients.Users(combinedUserIds).OnNotificationCreated(notifResponse);
            }

            await Task.CompletedTask;

        }

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

        #endregion
    }
}
