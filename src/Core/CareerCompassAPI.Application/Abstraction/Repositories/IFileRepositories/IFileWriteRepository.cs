using CareerCompassAPI.Domain.Entities;
using File = CareerCompassAPI.Domain.Entities.File;

namespace CareerCompassAPI.Application.Abstraction.Repositories.IFileRepositories
{
    public interface IFileWriteRepository:IWriteRepository<File>
    {
    }
}
