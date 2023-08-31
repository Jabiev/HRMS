using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CareerCompassAPI.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }
        private string GenerateGroupId(string userId1, string userId2)
        {
            var list = new List<string> { userId1, userId2 };
            list.Sort();
            return String.Join("_", list);
        }
        public async Task SendMessageAsync(string senderId, string recipientId, string message)
        {
            var groupId = GenerateGroupId(senderId, recipientId);
            _logger.LogInformation($"Sending message to Group: {groupId}, Sender: {senderId}, Recipient: {recipientId} Message: {message}");
            await Clients.Group(groupId).SendAsync("ReceiveMessage", senderId, recipientId, message);
        }

        public async Task JoinGroup(string userId1, string userId2)
        {
            var groupId = GenerateGroupId(userId1, userId2);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task LeaveGroup(string userId1, string userId2)
        {
            var groupId = GenerateGroupId(userId1, userId2);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }
    }
}
