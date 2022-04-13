namespace WebRazor.ViewModels.Security
{
    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public IEnumerable<ClaimInfo> Claims { get; set; }
    }

    public class ClaimInfo
    {
        public string ClaimType { get; set; }
        public string Value { get; set; }
    }
}
