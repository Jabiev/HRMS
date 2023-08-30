using Microsoft.AspNetCore.SignalR;

namespace CareerCompassAPI.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderId, string recipientId, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", senderId, recipientId, message);
        }
    }
}
