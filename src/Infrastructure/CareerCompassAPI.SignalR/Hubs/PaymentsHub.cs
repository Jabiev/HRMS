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
            _logger.LogInformation("Sending PaymentSuccess message to all clients."); // Assuming _logger is your logger instance
            await Clients.All.SendAsync("PaymentSuccess");
        }

    }
}
