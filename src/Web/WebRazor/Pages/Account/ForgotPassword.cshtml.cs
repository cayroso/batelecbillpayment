using Data.Identity.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace WebRazor.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityWebUser> _userManager;
        private readonly IEmailSender _emailSender;
        readonly IConfiguration _configuration;
        public ForgotPasswordModel(UserManager<IdentityWebUser> userManager, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null)// || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code, email = user.Email },
                    protocol: Request.Scheme);

                // send to phone number
                var fromPhoneNumber = _configuration["Twilio:DefaultPhoneNumber"];
                var en = Environment.NewLine;
                var message = $"{en}{en}Your email {user.Email} has requested for a password reset.{en}Please reset your password by clicking this link, {callbackUrl}.";
                var toPhoneNumber = user.PhoneNumber;

                try
                {
                    await SendSms(fromPhoneNumber, message, toPhoneNumber);
                }
                catch (Exception ex)
                {
                    // swallow exception
                }
                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }

        async Task SendSms(string fromPhoneNumber, string message, string toPhoneNumber)
        {
            var accountSid = _configuration["Twilio:AccountSID"];
            var authToken = _configuration["Twilio:AuthToken"];
            TwilioClient.Init(accountSid, authToken);

            try
            {
                var xxx = MessageResource.Create(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(fromPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(toPhoneNumber)
                );

            }
            catch (Exception ex)
            {

                throw;
            }


        }
    }
}
