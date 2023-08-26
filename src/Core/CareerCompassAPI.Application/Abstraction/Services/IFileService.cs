using CareerCompassAPI.Application.DTOs.File_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IFileService
    {
        Task CreateAsync(FileCreateDto fileCreateDto);
    }
}
