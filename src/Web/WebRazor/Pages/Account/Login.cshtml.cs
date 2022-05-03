using App.Services;
using Data.Constants;
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

        public async Task<IActionResult> OnPost([FromServices] NotificationService _notificationService, string returnUrl, CancellationToken cancellationToken)
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

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, true);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    ErrorMessage = "Account is locked out.";

                    await _notificationService.DeleteNotificationByReferenceId(user.Id, cancellationToken);

                    await _notificationService.AddNotification(user.Id, "error", $"User [{Input.Email}] Locked",
                        $"{Input.Email} was locked at {DateTime.Now}.", DateTime.UtcNow,
                        Data.Identity.Models.Notifications.EnumNotificationType.Error, Data.Identity.Models.Notifications.EnumNotificationEntityClass.Unknown,
                        Array.Empty<string>(), new[] { ApplicationRoles.System.Name }, cancellationToken);

                }
                else if (signInResult.IsNotAllowed)
                    ErrorMessage = "Account not allowed to access the system.";

                else if (signInResult.RequiresTwoFactor)
                    ErrorMessage = "System requires two factor authentication.";

                else
                    ErrorMessage = "Invalid username/email or password.";


            }

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
                return Page();

            await _signInManager.SignInAsync(user, Input.RememberMe);


            var isSystem = await _userManager.IsInRoleAsync(user, ApplicationRoles.System.Name);
            var isAdmin = await _userManager.IsInRoleAsync(user, ApplicationRoles.Administrator.Name);
            var isConsumer = await _userManager.IsInRoleAsync(user, ApplicationRoles.Consumer.Name);

            var onlySystem = isSystem && !isAdmin && !isConsumer;
            var onlyAdmin = !isSystem && isAdmin && !isConsumer;
            var onlyConsumer = !isSystem && !isAdmin && isConsumer;
            
            if (!string.IsNullOrWhiteSpace(returnUrl))
                return LocalRedirect(returnUrl);

            if (onlySystem)
                return Redirect("/system");
            else if (onlyAdmin)
                return Redirect("/administrator");
            else if (onlyConsumer)
                return Redirect("/consumer");

            return RedirectToPage("/Index");
        }
    }


}
