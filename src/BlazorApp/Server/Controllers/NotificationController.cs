
using Data.Identity.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : BaseController
    {
        IdentityWebContext _identityWebContext;
        public NotificationController(IdentityWebContext identityWebContext)
        {
            _identityWebContext = identityWebContext;
        }

        [HttpGet("my-notifications")]
        public async Task<IActionResult> GetMyNotifications()
        {
            //var dto = await _identityWebContext.Accounts

            return Ok();
        }
    }
}
