using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blazor.Shared;
using Data.Identity.Models.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Blazor.Shared.Security;
using Data.Identity.DbContext;
using Data.Constants;

namespace Blazor.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly UserManager<IdentityWebUser> _userManager;
        private readonly SignInManager<IdentityWebUser> _signInManager;

        public AuthorizeController(UserManager<IdentityWebUser> userManager, SignInManager<IdentityWebUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel parameters)
        {
            var user = await _userManager.FindByEmailAsync(parameters.Email);
            if (user == null) return BadRequest("User does not exist");
            var singInResult = await _signInManager.CheckPasswordSignInAsync(user, parameters.Password, false);
            if (!singInResult.Succeeded) return BadRequest("Invalid password");

            await _signInManager.SignInAsync(user, parameters.RememberMe);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Register(
            [FromServices] IdentityWebContext identityWebContext, 
            RegisterModel parameters)
        {
            var user = new IdentityWebUser();
            user.Id = Guid.NewGuid().ToString();
            user.Email = parameters.Email;
            user.UserName = parameters.Email;

            var result = await _userManager.CreateAsync(user, parameters.Password);
            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);

            var userRoles = new List<IdentityUserRole<string>>(new[]{
                    new IdentityUserRole<string> {
                        UserId = user.Id,
                        RoleId = ApplicationRoles.Administrator.Id
                    },
                    new IdentityUserRole<string> {
                        UserId = user.Id,
                        RoleId = ApplicationRoles.Manager.Id
                    },
                    new IdentityUserRole<string> {
                        UserId = user.Id,
                        RoleId = ApplicationRoles.Member.Id
                    },
                });

            await identityWebContext.AddRangeAsync(userRoles);

            await identityWebContext.SaveChangesAsync();

            return await Login(new LoginModel
            {
                Email = parameters.Email,
                Password = parameters.Password
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
