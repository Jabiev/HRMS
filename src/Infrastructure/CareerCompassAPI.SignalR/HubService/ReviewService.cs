using CareerCompassAPI.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CareerCompassAPI.SignalR.HubService
{
    public class ReviewService
    {
        private readonly IHubContext<ReviewHub> _hubContext;

        public ReviewService(IHubContext<ReviewHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void ApproveReview(Guid reviewId)
        {

            _hubContext.Clients.All.SendAsync("ReviewApproved");
        }
    }

}
