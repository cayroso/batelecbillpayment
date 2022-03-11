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
using BlazorApp.Shared.Branches;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BranchController : BaseController
    {
        IdentityWebContext _identityWebContext;
        public BranchController(IdentityWebContext identityWebContext)
        {
            _identityWebContext = identityWebContext;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> Get()
        {
            var dto = await _identityWebContext.Branches
                .Select(e => new BranchInfo
                {
                    BranchId = e.BranchId,
                    Name = e.Name
                })
                .ToListAsync();

            return Ok(dto);


        }

    }


}
