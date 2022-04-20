using App.Services;
using WebRazor.ViewModels.Announcements;
using Data.Identity.DbContext;
using Data.Identity.Models.Announcements;
using Data.Identity.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cayent.Core.Common.Extensions;

namespace WebRazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnnouncementController : BaseController
    {
        IdentityWebContext _identityWebContext;
        readonly NotificationService _notificationService;
        public AnnouncementController(IdentityWebContext identityWebContext,
            NotificationService notificationService
            )
        {
            _identityWebContext = identityWebContext;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string c, int p, int s, string sf, int so, CancellationToken cancellationToken)
        {
            var sql = from n in _identityWebContext.Announcements
                      select new ViewAnnouncementInfo
                      {
                          AnnouncementId = n.AnnouncementId,
                          Subject = n.Subject,
                          //Content = n.Content,
                          DateCreated = n.DateCreated,
                      };

            var dto = await sql.ToPagedItemsAsync(p, s, cancellationToken);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var sql = from n in _identityWebContext.Announcements
                      where n.AnnouncementId == id
                      select new ViewAnnouncementInfo
                      {
                          AnnouncementId = n.AnnouncementId,
                          Subject = n.Subject,
                          Content = n.Content,
                          DateCreated = n.DateCreated,
                      };

            var dto = await sql.FirstOrDefaultAsync();

            if (dto == null)
                return BadRequest("Announcement not found.");

            return Ok(dto);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Post([FromBody] AddAnnouncementInfo info, CancellationToken cancellationToken)
        {
            var data = new Announcement
            {
                AnnouncementId = GuidStr(),
                Subject = info.Subject,
                Content = info.Content,
                DatePost = info.DatePost.Date,
                DateCreated = DateTime.UtcNow
            };

            await _identityWebContext.AddAsync(data, cancellationToken);

            await _identityWebContext.SaveChangesAsync(cancellationToken);

            await _notificationService.AddNotification(data.AnnouncementId, "info", "New Announcement", data.Subject, data.DateCreated,
                    EnumNotificationType.Success, EnumNotificationEntityClass.Announcement, Array.Empty<string>(), new[] { "Consumer" }, cancellationToken);

            return Ok(data.AnnouncementId);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{announcementId}")]
        public async Task<IActionResult> Remove(string announcementId)
        {
            var data = await _identityWebContext.Announcements
                        .FirstOrDefaultAsync(e => e.AnnouncementId == announcementId);


            if (data != null)
            {
                _identityWebContext.Remove(data);

                var notif = await _identityWebContext.Notifications.Include(e => e.Receivers)
                    .FirstOrDefaultAsync(e => e.ReferenceId == data.AnnouncementId);

                if (notif != null)
                {
                    _identityWebContext.Remove(notif);
                }

                await _identityWebContext.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
