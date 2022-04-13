using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRazor.ViewModels.Announcements
{
    public class ViewAnnouncementInfo
    {
        public string AnnouncementId { get; set; }        
        public string Subject { get; set; }
        public string Content { get; set; }        
        public DateTime DateCreated { get; set; }
    }
}
