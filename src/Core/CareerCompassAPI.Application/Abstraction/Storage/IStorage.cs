using CareerCompassAPI.Application.DTOs.File_DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareerCompassAPI.Application.Abstraction.Storage
{
    public interface IStorage
    {
        Task<List<FileUploadResponseDto>> UploadAsync(FileUploadDto fileUploadDto); 
        Task DeleteAsync(string pathOrContainerName, string fileName);
        List<string> GetFiles(string pathOrContainerName);
        bool HasFile(string pathOrContainerName, string fileName);
    }
}
