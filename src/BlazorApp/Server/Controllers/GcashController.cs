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

namespace Blazor.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GcashController : ControllerBase
    {
        private readonly UserManager<IdentityWebUser> _userManager;
        private readonly SignInManager<IdentityWebUser> _signInManager;
        IdentityWebContext _identityWebContext;
        public GcashController(IdentityWebContext identityWebContext, UserManager<IdentityWebUser> userManager, SignInManager<IdentityWebUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityWebContext = identityWebContext;
        }

        [HttpPost("webhook")]
        //[HttpGet("webhook")]
        public async Task WebHook()
        {
            var foo = Request;            
        }

    }


}
