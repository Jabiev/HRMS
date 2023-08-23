using CareerCompassAPI.Application.Abstraction.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CareerCompassAPI.Persistence.ExtensionMethods
{
    public static class HangFireExtension
    {
        public static void CheckSubscriptionJob(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var hangFireService = scope.ServiceProvider.GetRequiredService<IHangFireService>();
                hangFireService.CheckSubscriptions().Wait();
            }
        }
    }
}
