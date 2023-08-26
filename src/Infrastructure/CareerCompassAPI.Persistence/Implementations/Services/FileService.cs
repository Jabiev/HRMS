using CareerCompassAPI.Application.Abstraction.Repositories.IFileRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.File_DTOs;
using CareerCompassAPI.Persistence.Contexts;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class FileService : IFileService
    {
        private readonly CareerCompassDbContext _context;
        private readonly IFileWriteRepository _fileWriteRepository;

        public FileService(CareerCompassDbContext context, IFileWriteRepository fileWriteRepository)
        {
            _context = context;
            _fileWriteRepository = fileWriteRepository;
        }

        public Task CreateAsync(FileCreateDto fileCreateDto)
        {
            throw new NotImplementedException();
        }
    }
}
