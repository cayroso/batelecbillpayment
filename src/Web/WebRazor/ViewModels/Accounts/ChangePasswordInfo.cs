namespace WebRazor.ViewModels.Accounts
{
    public class ChangePasswordInfo
    {
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        //public string ConfirmPassword { get; set; }
    }
}
