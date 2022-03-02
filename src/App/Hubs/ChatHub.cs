using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Chats;

namespace App.Hubs
{
    public interface IChatClient
    {
        Task ChatMessageReceived(ChatMessageReceivedInfo info);

        Task ChatMessageReceivedFromGroup(ChatMessageReceivedInfo info);

        Task ChatReceiverAdded(ChatReceiverAddedInfo info);

        Task ChatReceiverRemoved(ChatReceiverRemovedInfo info);

        Task ChatDeleted(string chatId);
    }

    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        public async Task JoinChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task LeaveChat(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }
    }
}
