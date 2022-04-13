using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blazor.Shared;
using Data.Identity.Models.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Blazor.Shared.Security;
using Data.Identity.DbContext;
using Data.Constants;
using Microsoft.EntityFrameworkCore;
using WebRazor.ViewModels.GCash;
using WebRazor.ViewModels.Billing;
using Data.Identity.Models;
using WebRazor.ViewModels.Reservations;
using Data.Identity.Models.Reservations;
using Cayent.Core.Common.Extensions;
using App.Services;

namespace WebRazor.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReservationController : BaseController
    {
        IdentityWebContext _identityWebContext;
        readonly NotificationService _notificationService;
        public ReservationController(IdentityWebContext identityWebContext, NotificationService notificationService)
        {
            _identityWebContext = identityWebContext;
            _notificationService = notificationService;
        }

        [HttpGet("{reservationId}")]
        public async Task<IActionResult> Get(string reservationId)
        {
            var dto = await _identityWebContext.Reservations
                .Where(e => e.AccountId == UserId && e.ReservationId == reservationId)
                .Select(e => new ReservationInfo
                {
                    ReservationId = e.ReservationId,
                    DateReservation = e.DateReservation,
                    BranchId = e.BranchId,
                    BranchName = e.Branch.Name,
                    AccountId = e.AccountId,
                    AccountName = $"{e.Account.UserInformation.FirstLastName}"
                })
                .ToListAsync();

            return Ok(dto);


        }

        [HttpGet("my-reservations")]
        public async Task<IActionResult> GetMyReservations()
        {
            var dto = await _identityWebContext.Reservations
                .Where(e => e.AccountId == UserId)
                .Select(e => new ReservationInfo
                {
                    ReservationId = e.ReservationId,
                    DateReservation = e.DateReservation,
                    BranchId = e.BranchId,
                    BranchName = e.Branch.Name,
                    AccountId = e.AccountId,
                    AccountName = $"{e.Account.UserInformation.FirstLastName}"
                })
                .ToListAsync();

            return Ok(dto);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetReservations()
        {
            var dto = await _identityWebContext.Reservations
                .Select(e => new ReservationInfo
                {
                    ReservationId = e.ReservationId,
                    DateReservation = e.DateReservation,
                    BranchId = e.BranchId,
                    BranchName = e.Branch.Name,
                    AccountId = e.AccountId,
                    AccountName = $"{e.Account.UserInformation.FirstLastName}"
                })
                .ToListAsync();

            return Ok(dto);
        }
        //[HttpGet("DateTimeSlots/{dateTicks}")]
        //public async Task<IActionResult> GetTimeSlotsForDate(long dateTicks)
        //{
        //    var dateUtc = new DateTime(dateTicks).AsUtc().Truncate();

        //    var reservedTimeSpans = await _identityWebContext.Reservations
        //        .Where(e => e.DateReservation.Date == dateUtc.Date)
        //        .Select(e => e.DateReservation.ToString("hh:mm tt"))
        //        .ToListAsync();

        //    var now = DateTime.Now.Date.AddHours(9);
        //    var end = now.AddHours(9);

        //    var timeSlots = new List<TimeSlot>();

        //    for (; now < end; now = now.AddMinutes(15))
        //    {
        //        var slot = now.ToString("hh:mm tt");
        //        timeSlots.Add(new TimeSlot
        //        {
        //            Slot = slot,
        //            Reserved = reservedTimeSpans.Any(e => e == slot)
        //        });


        //    }

        //    return Ok(timeSlots);
        //}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(AddReservationInfo info, CancellationToken cancellationToken)
        {
            //var ts = DateTime.Parse(info.TimeSlot);
            //var dateReservation = info.DateReservation.Date.AddHours(ts.Hour).AddMinutes(ts.Minute).Truncate();
            //dateReservation = DateTime.SpecifyKind(dateReservation, DateTimeKind.Local);

            var dateReservation = info.DateReservation;

            //  check if date is in the past
            var now = DateTime.UtcNow;

            if (dateReservation.Date < now.Date)
                return BadRequest("Cannot place reservation in the past.");

            //  check if the existing            
            var exists = await _identityWebContext.Reservations.AnyAsync(e => e.BranchId == info.BranchId && e.DateReservation == dateReservation, cancellationToken);

            if (exists)
                return BadRequest("Time slot already reserved.");

            var data = new Reservation
            {
                ReservationId = GuidStr(),
                AccountId = UserId,
                BranchId = info.BranchId,
                DateReservation = dateReservation,
            };

            await _identityWebContext.AddAsync(data, cancellationToken);

            await _identityWebContext.SaveChangesAsync(cancellationToken);

            //  send notif to admins
            var branch = await _identityWebContext.Branches.FirstAsync(e => e.BranchId == info.BranchId, cancellationToken);

            await _notificationService.AddNotification(data.ReservationId, "info", "Reservation was created",
                $"New reservation in {branch.Name} at {data.DateReservation} was created by consumer: {User.Identity.Name}", DateTime.UtcNow,
                Data.Identity.Models.Notifications.EnumNotificationType.Info, Data.Identity.Models.Notifications.EnumNotificationEntityClass.Reservation,
                Array.Empty<string>(), new[] { ApplicationRoles.Administrator.Name }, cancellationToken);

            return Ok();
        }

        [HttpDelete("{reservationId}/{reason}")]
        public async Task<IActionResult> Delete(string reservationId, string reason, CancellationToken cancellationToken)
        {

            var data = await _identityWebContext.Reservations
                .Include(e => e.Branch)
                .FirstOrDefaultAsync(e => e.ReservationId == reservationId, cancellationToken);

            if (data == null)
            {
                return BadRequest("Reservation not found.");
            }

            if (data.AccountId != UserId && !User.IsInRole("Administrator"))
            {
                return BadRequest("Only administrator can delete this reservation.");
            }

            _identityWebContext.Remove(data);

            await _identityWebContext.SaveChangesAsync(cancellationToken);

            var isAdmin = User.IsInRole("Administrator");
            var users = isAdmin ? new[] { data.AccountId } : Array.Empty<string>();
            var roles = isAdmin ? Array.Empty<string>() : new[] { "Administrator" };

            var content = isAdmin ?
                $"Your reservation in {data.Branch.Name} at {data.DateReservation} was deleted by Administrator[{User.Identity.Name}]. Reason: {reason}"
                : $"Consumer[{User.Identity.Name}] reservation in {data.Branch.Name} at {data.DateReservation} was deleted. Reason: {reason}";

            //  send notif to consumer
            await _notificationService.AddNotification(reservationId, "danger", "Reservation was deleted",
                content, DateTime.UtcNow,
                Data.Identity.Models.Notifications.EnumNotificationType.Info, Data.Identity.Models.Notifications.EnumNotificationEntityClass.Reservation,
                users, roles, cancellationToken);

            return Ok();
        }
    }


}
