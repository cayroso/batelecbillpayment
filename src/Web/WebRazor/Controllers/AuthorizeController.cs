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
using Data.Identity.Models;

namespace WebRazor.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : BaseController
    {
        private readonly UserManager<IdentityWebUser> _userManager;
        private readonly SignInManager<IdentityWebUser> _signInManager;
        IdentityWebContext _identityWebContext;
        public AuthorizeController(IdentityWebContext identityWebContext, UserManager<IdentityWebUser> userManager, SignInManager<IdentityWebUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityWebContext = identityWebContext;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel parameters)
        {
            var u = await _identityWebContext.Users.FirstOrDefaultAsync(e => e.Email == parameters.Email);
            var ui = await _identityWebContext.UserInformations.FirstOrDefaultAsync(e => e.UserId == u.Id);

            var user = await _userManager.FindByEmailAsync(parameters.Email);
            if (user == null) return BadRequest("User does not exist");
            var singInResult = await _signInManager.CheckPasswordSignInAsync(user, parameters.Password, false);
            if (!singInResult.Succeeded)
            {
                if(singInResult.IsLockedOut)
                    return BadRequest("Account is locked out.");
                
                if (singInResult.IsNotAllowed)
                    return BadRequest("Account not allowed to access the system.");

                if (singInResult.RequiresTwoFactor)
                    return BadRequest("System requires two factor authentication.");

                return BadRequest("Invalid username/email or password.");
            }

            await _signInManager.SignInAsync(user, parameters.RememberMe);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Register(
            [FromServices] IdentityWebContext identityWebContext,
            RegisterModel info)
        {
            var user = new IdentityWebUser();
            user.Id = GuidStr();
            user.TenantId = "administrator";
            user.Email = info.Email;
            user.UserName = info.Email;
            user.PhoneNumber = info.PhoneNumber;

            var result = await _userManager.CreateAsync(user, info.Password);
            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);

            var userInfo = new UserInformation
            {
                //UserInformationId = Guid.NewGuid().ToString(),
                UserId = user.Id,
                FirstName = info.FirstName,
                MiddleName = info.MiddleName,
                LastName = info.LastName,
            };

            var account = new Account
            {
                AccountId = user.Id,
                AccountNumber = "ACC#-" + GuidStr(),
                ConsumerType = "Residential",
                MeterNumber = info.MeterNumber,
                Address = info.Address,
            };

            await _userManager.AddToRoleAsync(user, "Consumer");

            await identityWebContext.AddRangeAsync(userInfo, account);

            await identityWebContext.SaveChangesAsync();

            return await Login(new LoginModel
            {
                Email = info.Email,
                Password = info.Password
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        public UserInfo UserInfo()
        {
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            return BuildUserInfo();
        }


        private UserInfo BuildUserInfo()
        {
            var userInfo = new UserInfo
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                Claims = User.Claims.Select(e => new ClaimInfo { ClaimType = e.Type, Value = e.Value })
                //Optionally: filter the claims you want to expose to the client
                //.Where(c => c.Type == "test-claim")
                //.Where(e=> e.Type != ClaimTypes.Role)
                //.ToDictionary(c => c.Type, c => c.Value)
            };

            return userInfo;
        }
    }


}
