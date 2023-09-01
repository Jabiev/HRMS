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
            _logger.LogInformation($"Sending message to Group: {groupId}, Sender: {senderId}, Recipient: {recipientId}, Message: {message}");

            await Clients.GroupExcept(groupId, new List<string> { Context.ConnectionId }).SendAsync("ReceiveMessage", senderId, recipientId, message);
        }


        public async Task JoinGroup(string userId1, string userId2)
        {
            var groupId = GenerateGroupId(userId1, userId2);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            _logger.LogInformation($"User {Context.ConnectionId} has joined the group {groupId}");
        }

        public async Task LeaveGroup(string userId1, string userId2)
        {
            var groupId = GenerateGroupId(userId1, userId2);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }
        public async Task SendCallOfferAsync(string senderId, string recipientId, string offer)
        {
            var groupId = GenerateGroupId(senderId, recipientId);
            _logger.LogInformation($"Sending call offer from {senderId} to {recipientId} in group {groupId}");
            await Clients.GroupExcept(groupId, new List<string> { Context.ConnectionId }).SendAsync("ReceiveCallOffer", senderId, recipientId, offer);
        }

        public async Task SendCallAnswerAsync(string senderId, string recipientId, string answer)
        {
            var groupId = GenerateGroupId(senderId, recipientId);
            _logger.LogInformation($"Sending call answer from {senderId} to {recipientId} in group {groupId}");
            await Clients.GroupExcept(groupId, new List<string> { Context.ConnectionId }).SendAsync("ReceiveCallAnswer", senderId, recipientId, answer);
        }

        public async Task SendIceCandidateAsync(string senderId, string recipientId, string iceCandidate)
        {
            var groupId = GenerateGroupId(senderId, recipientId);
            _logger.LogInformation($"Sending ICE candidate from {senderId} to {recipientId} in group {groupId}");
            await Clients.GroupExcept(groupId, new List<string> { Context.ConnectionId }).SendAsync("ReceiveIceCandidate", senderId, recipientId, iceCandidate);
        }
    }
}
