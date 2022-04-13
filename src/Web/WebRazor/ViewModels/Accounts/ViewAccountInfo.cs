namespace WebRazor.ViewModels.Accounts
{
    public class ViewUserInfo
    {
        public string UserId { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public ViewAccountInfo AccountInfo { get; set; } = new();

        public class ViewAccountInfo
        {
            public string AccountNumber { get; set; }
            public string MeterNumber { get; set; }
            public string ConsumerType { get; set; }
            public string Address { get; set; }
        }
    }


}
