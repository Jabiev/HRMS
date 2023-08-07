using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CareerCompassAPI.Persistence.Interceptors
{
    public class GuidGeneratorInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            GenerateIdProperties(eventData.Context.ChangeTracker.Entries());
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void GenerateIdProperties(IEnumerable<EntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    foreach (var property in entry.Properties)
                    {
                        if (property.Metadata.Name == "Id" && property.CurrentValue == null)
                        {
                            property.CurrentValue = Guid.NewGuid();
                            break;
                        }
                    }
                }
            }
        }
    }
}
