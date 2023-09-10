using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.RTC_DTOs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace CareerCompassAPI.SignalR.Hubs
{

    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private static readonly ConcurrentDictionary<string, string> UserConnectionMap = new ConcurrentDictionary<string, string>();
        private readonly IMessageService _messageService;

        public ChatHub(ILogger<ChatHub> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        public override async Task OnConnectedAsync()
        {
            string userId = Context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;


            if (userId != null)
            {
                UserConnectionMap.TryAdd(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string userId;
            UserConnectionMap.TryRemove(Context.ConnectionId, out userId);
            await base.OnDisconnectedAsync(exception);
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

            var unreadMessages = await _messageService.GetUnreadMessagesAsync(userId1, userId2);

            await Clients.Caller.SendAsync("ReceiveUnreadMessages", unreadMessages);
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
        public async Task StartNewVideoCall(string callerId, string recipientId, string offer)
        {
            var groupId = GenerateGroupId(callerId, recipientId);
            _logger.LogInformation($"Starting a new video call between {callerId} and {recipientId} in group {groupId}");

            await Clients.GroupExcept(groupId, new List<string> { Context.ConnectionId }).SendAsync("ReceiveCallOffer", callerId, recipientId, offer);
        }
        public async Task SendIceCandidateAsync(string senderId, string recipientId, string iceCandidate)
        {
            var groupId = GenerateGroupId(senderId, recipientId);
            _logger.LogInformation($"Sending ICE candidate from {senderId} to {recipientId} in group {groupId}");
            await Clients.GroupExcept(groupId, new List<string> { Context.ConnectionId }).SendAsync("ReceiveIceCandidate", senderId, recipientId, iceCandidate);
        }

        public async Task StartDirectCallAsync(string userId, string recipientId, string offerJson)
        {
            try
            {
                UserConnectionMap.TryAdd(Context.ConnectionId, userId);
                RTCSessionDescriptionDTO offer = JsonConvert.DeserializeObject<RTCSessionDescriptionDTO>(offerJson);

                if (string.IsNullOrEmpty(offer.Type) || string.IsNullOrEmpty(offer.Sdp))
                {
                    _logger.LogWarning("Invalid offer received for starting a direct call.");
                    return;
                }
                await Clients.User(recipientId).SendAsync("ReceiveDirectCall", userId, recipientId, offerJson);

                _logger.LogInformation($"User {userId} started a direct call with {recipientId}. Offer forwarded.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while starting a direct call: {ex.Message}");
                throw;
            }
        }

    }
}
