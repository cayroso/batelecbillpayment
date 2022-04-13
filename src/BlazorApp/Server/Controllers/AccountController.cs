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
using Cayent.Core.Common.Extensions;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IdentityWebContext _identityWebContext;
        private readonly UserManager<IdentityWebUser> _userManager;

        public AccountController(IdentityWebContext identityWebContext, UserManager<IdentityWebUser> userManager)
        {
            _identityWebContext = identityWebContext;
            _userManager = userManager;
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetInfo()
        {
            var user = await _identityWebContext.Users.SingleOrDefaultAsync(e => e.Id == UserId);
            var userInfo = await _identityWebContext.UserInformations.SingleOrDefaultAsync(e => e.UserId == UserId);
            var account = await _identityWebContext.Accounts.SingleOrDefaultAsync(e => e.AccountId == UserId);

            if (user == null)
                return BadRequest("User not found.");

            if (userInfo == null)
                return BadRequest("User information not found.");

            var dto = new ViewUserInfo
            {
                UserId = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = userInfo.FirstName,
                MiddleName = userInfo.MiddleName,
                LastName = userInfo.LastName,
            };

            if (account != null)
            {
                dto.AccountInfo = new ViewUserInfo.ViewAccountInfo
                {
                    AccountNumber = account.AccountNumber,
                    Address = account.Address,
                    ConsumerType = account.ConsumerType,
                    MeterNumber = account.MeterNumber
                };
            }

            return Ok(dto);
        }

        [HttpPut("user-info")]
        public async Task<IActionResult> UpdateUserInformation(EditUserInformationInfo info, CancellationToken cancellationToken)
        {
            var user = await _identityWebContext.Users.SingleOrDefaultAsync(e => e.Id == info.UserId, cancellationToken);
            var userInfo = await _identityWebContext.UserInformations.SingleOrDefaultAsync(e => e.UserId == info.UserId, cancellationToken);

            if (user == null)
                return BadRequest("User not found.");

            if (userInfo == null)
                return BadRequest("User information not found.");

            user.PhoneNumber = info.PhoneNumber;

            userInfo.FirstName = info.FirstName;
            userInfo.MiddleName = info.MiddleName;
            userInfo.LastName = info.LastName;

            await _identityWebContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpPut("account-info")]
        public async Task<IActionResult> UpdateAccount(EditAccountInfo info, CancellationToken cancellationToken)
        {
            var account = await _identityWebContext.Accounts.SingleOrDefaultAsync(e => e.AccountId == info.UserId, cancellationToken);

            if (account == null)
                return BadRequest("Account not found.");

            account.AccountNumber = info.AccountNumber;
            account.MeterNumber = info.MeterNumber;
            account.ConsumerType = info.ConsumerType;
            account.Address = info.Address;

            await _identityWebContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromServices] UserManager<IdentityWebUser> userManager, ChangePasswordInfo info, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(info.UserId);

            var result = await userManager.ChangePasswordAsync(user, info.CurrentPassword, info.NewPassword);

            if (!result.Succeeded)
                return BadRequest(string.Join('.', result.Errors.Select(e => e.Description).ToArray()));

            return Ok();
        }

        [HttpPut("{userId}/lockout")]
        public async Task<IActionResult> LockoutUser(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("User not found.");

            //await _userManager.SetLockoutEnabledAsync(user, true);

            var now = DateTime.MaxValue.Truncate().AsUtc();

            await _userManager.SetLockoutEndDateAsync(user, now);

            return Ok();
        }

        [HttpPut("{userId}/unlockout")]
        public async Task<IActionResult> UnlockoutUser(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("User not found.");

            var now = DateTime.UtcNow.Truncate().AsUtc();

            await _userManager.SetLockoutEndDateAsync(user, now.AddDays(-1));

            return Ok();
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


        #region Consumers

        [HttpGet("consumers")]
        public async Task<IActionResult> GetConsumers(CancellationToken cancellationToken)
        {
            var dto = await _identityWebContext.Accounts
                .Select(e => new ViewConsumerInfo
                {
                    UserId = e.AccountId,
                    Email = e.UserInformation.User.Email,
                    PhoneNumber = e.UserInformation.User.PhoneNumber,
                    AccountNumber = e.AccountNumber,
                    FirstLastName = e.UserInformation.FirstLastName,
                    IsLocked = e.UserInformation.User.LockoutEnabled && e.UserInformation.User.LockoutEnd > DateTime.UtcNow.Truncate(),
                }).ToListAsync(cancellationToken);

            return Ok(dto);
        }

        [HttpGet("consumers/{userId}")]
        public async Task<IActionResult> GetConsumer(string userId, CancellationToken cancellationToken)
        {
            var dto = await _identityWebContext.Accounts
                .Where(e => e.AccountId == userId)
                .Select(e => new ViewConsumerInfo
                {
                    UserId = e.AccountId,
                    Email = e.UserInformation.User.Email,
                    PhoneNumber = e.UserInformation.User.PhoneNumber,
                    IsLocked = e.UserInformation.User.LockoutEnabled && e.UserInformation.User.LockoutEnd > DateTime.UtcNow.Truncate(),
                    AccountNumber = e.AccountNumber,
                    Address = e.Address,
                    ConsumerType = e.ConsumerType,
                    MeterNumber = e.MeterNumber,

                    FirstLastName = e.UserInformation.FirstLastName,
                    FirstName = e.UserInformation.FirstName,
                    MiddleName = e.UserInformation.MiddleName,
                    LastName = e.UserInformation.LastName,
                }).FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
                return NotFound("Consumer not found.");

            return Ok(dto);
        }

        #endregion

        #region Administrators

        [HttpPost("administrator/add")]
        public async Task<IActionResult> AddAdmin(
            [FromServices] IdentityWebContext identityWebContext,
            AddAdministratorInfo info,
            CancellationToken cancellationToken)
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


            await _userManager.AddToRoleAsync(user, ApplicationRoles.Administrator.Name);

            await identityWebContext.AddRangeAsync(userInfo);

            await identityWebContext.SaveChangesAsync(cancellationToken);


            return Ok();
        }

        [HttpGet("administrators")]
        public async Task<IActionResult> GetAdministrators(CancellationToken cancellationToken)
        {
            var userIds = await _identityWebContext.UserRoles
                .Where(e => e.RoleId == ApplicationRoles.Administrator.Id)
                .Select(e => e.UserId)
                .ToListAsync();

            var now = DateTime.UtcNow.Truncate().AsUtc();

            var dto = await _identityWebContext.Users
                .Where(e => userIds.Contains(e.Id))
                .Select(e => new ViewAdministratorInfo
                {
                    UserId = e.Id,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    FirstLastName = e.UserInformation.FirstLastName,
                    IsLocked = e.LockoutEnabled && e.LockoutEnd > now
                }).ToListAsync(cancellationToken);

            return Ok(dto);
        }

        [HttpGet("administrators/{userId}")]
        public async Task<IActionResult> GetAdministrator(string userId, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow.Truncate().AsUtc();

            var dto = await _identityWebContext.Users
                .Where(e => e.Id == userId)
                .Select(e => new ViewAdministratorInfo
                {
                    UserId = e.Id,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,

                    FirstLastName = e.UserInformation.FirstLastName,
                    FirstName = e.UserInformation.FirstName,
                    MiddleName = e.UserInformation.MiddleName,
                    LastName = e.UserInformation.LastName,
                    IsLocked = e.LockoutEnabled && e.LockoutEnd > now
                }).FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
                return NotFound("Consumer not found.");

            return Ok(dto);
        }

        #endregion
    }


}
