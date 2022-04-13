using System.ComponentModel.DataAnnotations;

namespace WebRazor.ViewModels.Announcements
{
    public class AddAnnouncementInfo
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime DatePost { get; set; }
    }
}
