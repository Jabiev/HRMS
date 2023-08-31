using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CareerCompassAPI.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task LeaveGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        public async Task SendMessageAsync(string senderId, string recipientId, string message)
        {
            await Clients.Group(recipientId).SendAsync("ReceiveMessage", senderId, recipientId, message);
            await Clients.Group(senderId).SendAsync("ReceiveMessage", senderId, recipientId, message);  
        }
    }
}
