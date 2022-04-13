using System.ComponentModel.DataAnnotations;

namespace WebRazor.ViewModels.Notifications
{
    public class AddNotificationInfo
    {
        public string IconClass { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime DateSent { get; set; }
        public string RefLink { get; set; }
    }
}
