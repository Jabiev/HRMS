using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CareerCompassAPI.SignalR.Hubs
{
    public class PaymentHub : Hub
    {
        private readonly ILogger<PaymentHub> _logger;

        public PaymentHub(ILogger<PaymentHub> logger)
        {
            _logger = logger;
        }

        public async Task NotifyPaymentSuccess()
        {
            _logger.LogInformation("Sending PaymentSuccess message to all clients."); 
            await Clients.All.SendAsync("PaymentSuccess");
        }

    }
}
