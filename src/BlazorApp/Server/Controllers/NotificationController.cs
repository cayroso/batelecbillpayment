
using BlazorApp.Shared.Notifications;
using Data.Identity.DbContext;
using Data.Identity.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : BaseController
    {
        IdentityWebContext _identityWebContext;
        public NotificationController(IdentityWebContext identityWebContext)
        {
            _identityWebContext = identityWebContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var sql = from n in _identityWebContext.Notifications
                      select new ViewNotificationInfo
                      {
                          NotificationId = n.NotificationId,
                          Content = n.Content,
                          IconClass = n.IconClass,
                          RefLink = n.ReferenceId,
                          DateSent = n.DateSent,
                          Subject = n.Subject
                      };

            var dto = await sql.ToListAsync();

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
                          RefLink = nr.Notification.ReferenceId,
                          DateSent = nr.Notification.DateSent,
                          Subject = nr.Notification.Subject
                      };

            var dto = await sql.ToListAsync();

            return Ok(dto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Post([FromBody] AddNotificationInfo info)
        {            
            var data = new Notification
            {
                NotificationId = GuidStr(),
                Content = info.Content,
                DateSent = info.DateSent,
                IconClass = info.IconClass,
                ReferenceId = info.RefLink,
                Subject = info.Subject
            };

            var receivers = await _identityWebContext.Accounts.Select(e => new NotificationReceiver
            {
                NotificationId = data.NotificationId,
                ReceiverId = e.AccountId,
            }).ToListAsync();

            await _identityWebContext.AddAsync(data);
            await _identityWebContext.AddRangeAsync(receivers);

            await _identityWebContext.SaveChangesAsync();

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

        [HttpPut("{notificationId}/markAsRead")]
        public async Task<IActionResult> MarkAsRead(string notificationId)
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
    }
}
