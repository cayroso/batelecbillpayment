using System.ComponentModel.DataAnnotations;

namespace WebRazor.ViewModels.Accounts
{
    public class EditUserInformationInfo
    {
        [Required]
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
