using CareerCompassAPI.Application.DTOs.File_DTOs;
using Microsoft.AspNetCore.Http;

namespace CareerCompassAPI.Application.Abstraction.Storage
{
    public interface IStorage
    {
        Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(FileUploadDto fileUploadDto);
        Task DeleteAsync(string pathOrContainerName, string fileName);
        List<string> GetFiles(string pathOrContainerName);
        bool HasFile(string pathOrContainerName,string fileName);

    }
}
