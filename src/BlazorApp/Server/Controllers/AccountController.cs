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

namespace Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        IdentityWebContext _identityWebContext;
        public AccountController(IdentityWebContext identityWebContext)
        {
            _identityWebContext = identityWebContext;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> Get()
        {
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
