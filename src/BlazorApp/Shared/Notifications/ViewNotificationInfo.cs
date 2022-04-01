﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Notifications
{
    public class ViewNotificationInfo
    {
        public string NotificationId { get; set; }
        public string NotificationTypeText { get; set; }
        public string NotificationEntityClassText { get; set; }
        public string IconClass { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string ReferenceId { get; set; }
        public DateTime DateSent { get; set; }

        public bool IsRead { get; set; }
    }
}
