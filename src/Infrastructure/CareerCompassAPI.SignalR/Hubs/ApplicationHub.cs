namespace CareerCompassAPI.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

public class ApplicationHub : Hub
{
    private readonly ILogger<ApplicationHub> _logger;

    public ApplicationHub(ILogger<ApplicationHub> logger)
    {
        _logger = logger;
    }
    public async Task SendApplicationUpdate(int newCurrentApplicationCount)
    {
        _logger.LogInformation($"Sending Application Update: {newCurrentApplicationCount}");
        await Clients.All.SendAsync("ReceiveApplicationUpdate", newCurrentApplicationCount);
    }
}

