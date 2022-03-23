using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Announcements
{
    public class AddAnnouncementInfo
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
