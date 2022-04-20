using Data.Identity.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Hubs
{
    public class NotificationResponse
    {
        public string NotificationId { get; set; }
        public EnumNotificationType NotificationType { get; set; }
        public EnumNotificationEntityClass NotificationEntityClass { get; set; }
        public string NotificationEntityClassText => NotificationEntityClass.ToString();
        public string IconClass { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string ReferenceId { get; set; }
        public DateTime DateSent { get; set; }
    }

    public interface INotificationClient
    {
        Task OnNotificationCreated(NotificationResponse response);

        Task OnReservationCreated(NotificationResponse notification);
        Task OnReservationDeleted(NotificationResponse notification);

        Task OnBillingCreated(NotificationResponse notification);
        Task OnBillingPaid(NotificationResponse notification);
    }

    [Authorize]
    public class NotificationHub : Hub<INotificationClient>
    {
        public override Task OnConnectedAsync()
        {
            var identity = Context.User;

            Context.User.Claims
                .Where(p => p.Type == System.Security.Claims.ClaimTypes.Role)
                .ToList().ForEach(p =>
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, p.Value);
                });

            return base.OnConnectedAsync();
        }

        //public async Task SendNotificationCreated(string group, Notification notification)
        //{
        //    await Clients.GroupExcept(group, Context.ConnectionId).OnNotificationCreated(notification);
        //}

        //public async Task SendReservationCreated(string group, Notification notification)
        //{
        //    await Clients.GroupExcept(group, Context.ConnectionId).OnReservationCreated(notification);
        //}

        //public async Task SendReservationDeleted(string group, Notification notification)
        //{
        //    await Clients.GroupExcept(group, Context.ConnectionId).OnReservationDeleted(notification);
        //}

        //public async Task SendBillingCreated(string userId, Notification notification)
        //{
        //    await Clients.Users(userId).OnBillingCreated(notification);
        //}
        //public async Task SendBillingPaid(string group, Notification notification)
        //{
        //    await Clients.GroupExcept(group, Context.ConnectionId).OnBillingPaid(notification);
        //}
    }
}
