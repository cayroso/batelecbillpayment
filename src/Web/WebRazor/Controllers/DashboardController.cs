using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Data.Identity.DbContext;
using Microsoft.EntityFrameworkCore;
using WebRazor.ViewModels.Accounts;
using Cayent.Core.Common.Extensions;

namespace WebRazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : BaseController
    {
        IdentityWebContext _identityWebContext;
        public DashboardController(IdentityWebContext identityWebContext)
        {
            _identityWebContext = identityWebContext;
        }

        [HttpGet("systems")]
        public async Task<IActionResult> GetSystemDashboard()
        {
            // # of administrators
            // # of Consumers

            var dto = await _identityWebContext.Accounts
                .AsNoTracking()
                .Select(e => new AccountLookupInfo
                {
                    AccountId = e.AccountId,
                    Name = $"{e.AccountNumber} - {e.UserInformation.FirstLastName}"
                })
                .ToListAsync();

            return Ok(dto);
        }

        [HttpGet("administrators")]
        public async Task<IActionResult> GetAdministratorDashboard()
        {
            // no of consumers
            // no of reservations (today, tomorrow, this week)
            // no of billings (due date past, today, tomorrow, this week)

            // unread notifications

            var now = DateTime.UtcNow.Date;
            var tomorrow = now.AddDays(1);

            var monday = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Sunday).Truncate().AsUtc();
            var friday = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Saturday).AddDays(1).AddMinutes(-1).Truncate().AsUtc();

            //var noOfConsumers = await _identityWebContext.Accounts.CountAsync();

            var reservations = await _identityWebContext.Reservations
                .Where(e => e.DateReservation >= monday && e.DateReservation <= friday)
                .ToListAsync();

            var todayReservations = reservations.Count(e => e.DateReservation.Date == now.Date);
            var tomorrowReservations = reservations.Count(e => e.DateReservation.Date == tomorrow.Date);
            var weekReservations = reservations.Count();

            var billings = await _identityWebContext.Billings
                .Where(e => e.DateDue >= monday && e.DateDue <= friday)
                .ToListAsync();

            var pastDueDateBillings = await _identityWebContext.Billings
                .CountAsync(e => e.DateDue.Date < monday);
            var todayDueDateBillings = billings.Count(e => e.DateDue.Date == now.Date);
            var tomorrowDueDateBillings = billings.Count(e => e.DateDue.Date == tomorrow.Date);
            var weekDueDateBillings = billings.Count();

            var announcements = await _identityWebContext.Announcements
                .Where(e => e.DatePost >= monday && e.DatePost <= friday)
                .CountAsync();

            var notifications = await _identityWebContext.Notifications.Include(e => e.Receivers)
                .Where(e => e.DateSent >= monday && e.DateSent <= friday)
                .Where(e => e.Receivers.Any(e => e.ReceiverId == UserId))
                .CountAsync();

            return Ok(new ViewModels.Dashboards.Administrator
            {
                //noOfConsumers,
                TodayReservations = todayReservations,
                TomorrowReservations = tomorrowReservations,
                WeekReservations = weekReservations,

                PastDueDateBillings = pastDueDateBillings,
                TodayDueDateBillings = todayDueDateBillings,
                TomorrowDueDateBillings = tomorrowDueDateBillings,
                WeekDueDateBillings = weekDueDateBillings,

                Announcements = announcements,
                Notifications = notifications
            });
        }

        [HttpGet("consumers")]
        public async Task<IActionResult> GetConsumerDashboard()
        {
            // no of billings (overdue, today, tomorrow, this week)
            // no of reservations (overdue, today, tomorrow, this week)            
            var dto = await _identityWebContext.Accounts
                .AsNoTracking()
                .Select(e => new AccountLookupInfo
                {
                    AccountId = e.AccountId,
                    Name = $"{e.AccountNumber} - {e.UserInformation.FirstLastName}"
                })
                .ToListAsync();

            return Ok(dto);
        }

    }


}
