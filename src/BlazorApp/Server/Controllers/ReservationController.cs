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
using BlazorApp.Shared.GCash;
using BlazorApp.Shared.Billing;
using Data.Identity.Models;
using BlazorApp.Shared.Reservations;
using Data.Identity.Models.Reservations;
using Cayent.Core.Common.Extensions;

namespace Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReservationController : BaseController
    {
        IdentityWebContext _identityWebContext;
        public ReservationController(IdentityWebContext identityWebContext)
        {
            _identityWebContext = identityWebContext;
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
        public async Task<IActionResult> Post(AddReservationInfo info)
        {
            var ts = DateTime.Parse(info.TimeSlot);
            var dateReservation = info.DateReservation.Date.AddHours(ts.Hour).AddMinutes(ts.Minute).Truncate();
            dateReservation = DateTime.SpecifyKind(dateReservation, DateTimeKind.Local);

            //  check if date is in the past
            var now = DateTime.UtcNow;

            if (dateReservation <= now)
                return BadRequest("Cannot place reservation in the past.");

            //  check if the existing            
            var exists = await _identityWebContext.Reservations.AnyAsync(e => e.BranchId == info.BranchId && e.DateReservation == dateReservation);

            if (exists)
                return BadRequest("Time slot already reserved.");

            var data = new Reservation
            {
                ReservationId = GuidStr(),
                AccountId = UserId,
                BranchId = info.BranchId,
                DateReservation = dateReservation,
            };

            await _identityWebContext.AddAsync(data);

            await _identityWebContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{reservationId}")]
        public async Task<IActionResult> Delete(string reservationId)
        {

            var data = await _identityWebContext.Reservations.FirstOrDefaultAsync(e => e.ReservationId == reservationId);

            if (data == null || data.AccountId == UserId || !User.IsInRole("Administrator"))
            {
                return BadRequest("Reservation not found.");
            }

            _identityWebContext.Remove(data);

            await _identityWebContext.SaveChangesAsync();

            return Ok();
        }
    }


}
