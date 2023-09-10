using CareerCompassAPI.Application.Abstraction.Repositories.IFileRepositories;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.File_DTOs;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class FileService : IFileService
    {
        private readonly ICareerCompassDbContext _context;
        private readonly IFileWriteRepository _fileWriteRepository;

        public FileService(ICareerCompassDbContext context, IFileWriteRepository fileWriteRepository)
        {
            _context = context;
            _fileWriteRepository = fileWriteRepository;
        }
        public async Task CreateAsync(FileCreateDto fileCreateDto)
        {
            if (fileCreateDto == null) throw new ArgumentNullException();

            AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == fileCreateDto.appUserId);
            if (user is not AppUser)
            {
                throw new NotFoundException("User does not exist");
            }
            Domain.Entities.File newFile = new()
            {
                Name = fileCreateDto.name,
                BlobPath = fileCreateDto.blobPath,
                ContainerName = fileCreateDto.containerName,
                ContentType = fileCreateDto.contentType,
                Size = fileCreateDto.size,
                User = user,
            };
            await _fileWriteRepository.AddAsync(newFile);
            await _fileWriteRepository.SaveChangesAsync();
        }
    }
}
