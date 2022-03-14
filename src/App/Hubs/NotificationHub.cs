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
    public interface INotificationClient
    {
        Task Received(Notification notification);        
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
    }
}
