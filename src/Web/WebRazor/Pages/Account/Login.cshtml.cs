using Data.Identity.DbContext;
using Data.Identity.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityWebUser> _userManager;
        private readonly SignInManager<IdentityWebUser> _signInManager;
        private readonly IdentityWebContext _identityWebContext;

        public LoginModel(IdentityWebContext identityWebContext, UserManager<IdentityWebUser> userManager, SignInManager<IdentityWebUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityWebContext = identityWebContext;
        }

        [BindProperty]
        public ViewModels.Security.LoginModel Input { get; set; }
        [BindProperty]
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
                //var errors = ModelState.Values.Select(e => e.Errors.Select(n => n.ErrorMessage)).ToList();
                //ErrorMessage = string.Join(". ", errors);
                //return Page();
            }


            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ErrorMessage = "Invalid username/email or password.";
                return Page();
            }

            //var u = await _identityWebContext.Users.FirstOrDefaultAsync(e => e.Email == Email);
            //var ui = await _identityWebContext.UserInformations.FirstOrDefaultAsync(e => e.UserId == u.Id);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, false);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                    ErrorMessage = "Account is locked out.";

                if (signInResult.IsNotAllowed)
                    ErrorMessage = "Account not allowed to access the system.";

                if (signInResult.RequiresTwoFactor)
                    ErrorMessage = "System requires two factor authentication.";

                ErrorMessage = "Invalid username/email or password.";
            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
                return Page();

            await _signInManager.SignInAsync(user, Input.RememberMe);

            return RedirectToPage("/Index");
        }
    }


}
