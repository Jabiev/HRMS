using CareerCompassAPI.Application.Abstraction.Repositories.IFileRepositories;
using CareerCompassAPI.Persistence.Contexts;
using File = CareerCompassAPI.Domain.Entities.File;

namespace CareerCompassAPI.Persistence.Implementations.Repositories.FileRepositories
{
    public class FileWriteRepository : WriteRepository<File>, IFileWriteRepository
    {
        public FileWriteRepository(CareerCompassDbContext context) : base(context)
        {
        }
    }
}
