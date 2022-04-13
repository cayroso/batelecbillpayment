using Data.Constants;
using Data.Identity.DbContext;
using Data.Identity.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public WebRazor.ViewModels.Security.RegisterModel Input { get; set; }

        [BindProperty]
        public string ErrorMessages { get; set; }

        private readonly UserManager<IdentityWebUser> _userManager;
        private readonly SignInManager<IdentityWebUser> _signInManager;
        private readonly IdentityWebContext _identityWebContext;
        public RegisterModel(IdentityWebContext identityWebContext, UserManager<IdentityWebUser> userManager, SignInManager<IdentityWebUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityWebContext = identityWebContext;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = new IdentityWebUser();
            user.Id = Guid.NewGuid().ToString();
            user.TenantId = "administrator";
            user.Email = Input.Email;
            user.UserName = Input.Email;
            user.PhoneNumber = Input.PhoneNumber;

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                ErrorMessages = result.Errors.FirstOrDefault()?.Description;

                return Page();
            }

            var userInfo = new UserInformation
            {
                //UserInformationId = Guid.NewGuid().ToString(),
                UserId = user.Id,
                FirstName = Input.FirstName,
                MiddleName = Input.MiddleName,
                LastName = Input.LastName,
            };

            var account = new Data.Identity.Models.Account
            {
                AccountId = user.Id,
                AccountNumber = "ACC#-",//+ GuidStr(),
                ConsumerType = "Residential",
                MeterNumber = Input.MeterNumber,
                Address = Input.Address,
            };

            await _userManager.AddToRoleAsync(user, ApplicationRoles.Consumer.Name);

            await _identityWebContext.AddRangeAsync(userInfo, account);

            await _identityWebContext.SaveChangesAsync();

            return RedirectToPage("/Account/Login");            
        }
    }
}
