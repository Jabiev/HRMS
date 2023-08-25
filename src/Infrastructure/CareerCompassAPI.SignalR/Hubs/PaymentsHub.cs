using Microsoft.AspNetCore.SignalR;

namespace CareerCompassAPI.SignalR.Hubs
{
    public class PaymentHub : Hub
    {
        public async Task NotifyPaymentSuccess(string userId)
        {
            await Clients.User(userId).SendAsync("PaymentSuccess");
        }
    }

}
