using CareerCompassAPI.Application.Abstraction.Repositories.IFileRepositories;
using CareerCompassAPI.Persistence.Contexts;
using File = CareerCompassAPI.Domain.Entities.File;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.FileRepositories
{
    public class FileReadRepository : ReadRepository<File>, IFileReadRepository
    {
        public FileReadRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
