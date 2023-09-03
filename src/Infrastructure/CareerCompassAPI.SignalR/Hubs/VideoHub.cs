using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CareerCompassAPI.SignalR.Hubs
{
    public class VideoHub : Hub
    {
        private readonly ILogger<VideoHub> _logger;

        public VideoHub(ILogger<VideoHub> logger)
        {
            _logger = logger;
        }

        private string GenerateGroupId(string userId1, string userId2)
        {
            var list = new List<string> { userId1, userId2 };
            list.Sort();
            return String.Join("_", list);
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
            _logger.LogInformation($"User {Context.ConnectionId} has left the group {groupId}");
        }

        public async Task StartDirectCallAsync(string userId,string recipientId, string offerJson)
        {
            var groupId = GenerateGroupId(userId, recipientId);
            _logger.LogInformation($"Starting a new video call between {userId} and {recipientId} in group {groupId}");

            await JoinGroup(userId, recipientId);

            await Clients.GroupExcept(groupId, new List<string> { Context.ConnectionId }).SendAsync("ReceiveDirectCall", userId, recipientId, offerJson);
        }

        public async Task AnswerDirectCallAsync(string callerId, string answerJson)
        {
            var groupId = GenerateGroupId(Context.UserIdentifier, callerId);
            _logger.LogInformation($"Answer received. CallerId: {callerId}, AnswerJson: {answerJson}");

            await Clients.GroupExcept(groupId, new List<string> { Context.ConnectionId }).SendAsync("ReceiveDirectCallAnswer", callerId, answerJson);
        }
    }
}
