using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Data.Identity.DbContext;
using Microsoft.EntityFrameworkCore;
using WebRazor.ViewModels.Branches;

namespace WebRazor.Controllers
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
