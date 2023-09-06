using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CareerCompassAPI.SignalR.Hubs
{
    [Authorize]
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
            await JoinGroup(userId, recipientId);
            await Clients.GroupExcept(groupId, new List<string> { Context.ConnectionId }).SendAsync("ReceiveDirectCall", userId, recipientId, offerJson);
        }

        public async Task AnswerDirectCallAsync(string callerId, string answerJson)
        {
            var groupId = GenerateGroupId(Context.UserIdentifier, callerId);
            _logger.LogInformation($"Answer received. CallerId: {callerId}, AnswerJson: {answerJson}");

            _logger.LogInformation($"Current GroupID: {groupId}");
            _logger.LogInformation($"Current UserIdentifier: {Context.UserIdentifier}");
            _logger.LogInformation($"Current ConnectionId: {Context.ConnectionId}");

            await Clients.GroupExcept(groupId, new List<string> { Context.ConnectionId }).SendAsync("ReceiveDirectCallAnswer", callerId, answerJson);
        }
        public async Task SendIceCandidate(string recipientId, string iceCandidateJson)
        {
            var groupId = GenerateGroupId(Context.UserIdentifier, recipientId);
            await Clients.GroupExcept(groupId, new List<string> { Context.ConnectionId }).SendAsync("ReceiveIceCandidate", iceCandidateJson);
        }

        public async Task NotifyCallDeclined(string callerId)
        {
            await Clients.User(callerId).SendAsync("ReceiveCallDeclined", Context.UserIdentifier);
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.Features.Get<IHttpContextFeature>()?.HttpContext;
            if (httpContext != null)
            {
                var token = httpContext.Request.Query["access_token"];
                _logger.LogInformation($"Token received from query string: {token}");
            }
            else
            {
                _logger.LogWarning("HttpContext is null. Could not retrieve token.");
            }
            await base.OnConnectedAsync();
        }
    }
}
