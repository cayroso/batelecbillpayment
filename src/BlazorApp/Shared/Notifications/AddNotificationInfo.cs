using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Notifications
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
