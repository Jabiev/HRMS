using Microsoft.AspNetCore.SignalR;

namespace CareerCompassAPI.SignalR.Hubs;
public class ReviewHub : Hub
{
    public async Task ReviewApproved()
    {
        await Clients.All.SendAsync("ReviewApproved");
    }
}

