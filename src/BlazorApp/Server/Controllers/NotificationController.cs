
using App.Hubs;
using App.Services;
using BlazorApp.Shared.Notifications;
using Data.Identity.DbContext;
using Data.Identity.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : BaseController
    {
        IdentityWebContext _identityWebContext;
        //readonly IHubContext<NotificationHub, INotificationClient> _notificationHubContext;
        readonly NotificationService _notificationService;
        public NotificationController(IdentityWebContext identityWebContext,
            NotificationService notificationService
            //IHubContext<NotificationHub, INotificationClient> notificationHubContext
            )
        {
            _identityWebContext = identityWebContext;
            _notificationService = notificationService;
            //_notificationHubContext = notificationHubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var now = DateTime.UtcNow;

            var sql = from n in _identityWebContext.Notifications
                      where n.Receivers.Any(e => e.ReceiverId == UserId)
                      select new ViewNotificationInfo
                      {
                          NotificationId = n.NotificationId,
                          //Content = n.Content,
                          IconClass = n.IconClass,
                          ReferenceId = n.ReferenceId,
                          DateSent = n.DateSent,
                          Subject = n.Subject,
                          IsRead = n.Receivers.First(e => e.ReceiverId == UserId).DateRead < now
                      };

            var dto = await sql.ToListAsync();

            return Ok(dto);
        }

        [HttpGet("{notificationId}")]
        public async Task<IActionResult> GetNotifications(string notificationId, CancellationToken cancellationToken)
        {
            var data = await _identityWebContext.Notifications
                            .Include(e => e.Receivers)
                            .FirstOrDefaultAsync(e => e.NotificationId == notificationId);

            if (data == null)
                return BadRequest("Notification not found.");

            var dto = new ViewNotificationInfo
            {
                NotificationId = data.NotificationId,
                NotificationTypeText = data.NotificationType.ToString(),
                NotificationEntityClassText = data.NotificationEntityClass.ToString(),
                Content = data.Content,
                IconClass = data.IconClass,
                ReferenceId = data.ReferenceId,
                DateSent = data.DateSent,
                Subject = data.Subject,
            };

            var nr = data.Receivers.FirstOrDefault(e => e.ReceiverId == UserId);

            //  mark as read
            if (nr != null)
            {
                nr.DateRead = DateTime.UtcNow;

                await _identityWebContext.SaveChangesAsync(cancellationToken);

                dto.IsRead = true;
            }

            return Ok(dto);
        }

        [HttpGet("my-notifications/{unreadOnly}")]
        public async Task<IActionResult> GetMyNotifications(bool unreadOnly)
        {
            var sql = from nr in _identityWebContext.NotificationReceivers
                      where nr.ReceiverId == UserId
                      where !unreadOnly || nr.IsRead == false
                      select new ViewNotificationInfo
                      {
                          NotificationId = nr.Notification.NotificationId,
                          Content = nr.Notification.Content,
                          IconClass = nr.Notification.IconClass,
                          ReferenceId = nr.Notification.ReferenceId,
                          DateSent = nr.Notification.DateSent,
                          Subject = nr.Notification.Subject
                      };

            var dto = await sql.ToListAsync();

            return Ok(dto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Post([FromBody] AddNotificationInfo info, CancellationToken cancellationToken)
        {
            await _notificationService.AddNotification(info.RefLink, "info", info.Subject, info.Content, info.DateSent,
                    EnumNotificationType.Success, EnumNotificationEntityClass.Notification, Array.Empty<string>(), new[] { "Consumer" }, cancellationToken);

            return Ok();
        }

        [HttpDelete("{notificationId}/delete")]
        public async Task<IActionResult> Remove(string notificationId)
        {
            var data = await _identityWebContext.Notifications.Include(e => e.Receivers)
                        .FirstOrDefaultAsync(e => e.NotificationId == notificationId);

            if (data != null)
            {
                _identityWebContext.Remove(data);

                await _identityWebContext.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut("{notificationId}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(string notificationId)
        {
            var data = await _identityWebContext.NotificationReceivers
                        .FirstOrDefaultAsync(e => e.NotificationId == notificationId && e.ReceiverId == UserId);

            if (data != null)
            {
                data.DateRead = DateTime.UtcNow;
                //_identityWebContext.Remove(data);

                await _identityWebContext.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut("{notificationId}/unscribe")]
        public async Task<IActionResult> Unscribe(string notificationId)
        {
            var data = await _identityWebContext.NotificationReceivers
                        .FirstOrDefaultAsync(e => e.NotificationId == notificationId && e.ReceiverId == UserId);

            if (data != null)
            {
                _identityWebContext.Remove(data);

                await _identityWebContext.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
