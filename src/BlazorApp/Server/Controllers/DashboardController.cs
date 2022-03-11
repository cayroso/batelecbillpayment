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
using BlazorApp.Shared.Accounts;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            // 
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
