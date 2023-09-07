using CareerCompassAPI.Application.Abstraction.Storage;
using CareerCompassAPI.Application.DTOs.File_DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareerCompassAPI.Infrastructure.Services
{
    public class StorageService : IStorageService
    {
        private readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
            => await _storage.DeleteAsync(pathOrContainerName, fileName);

        public List<string> GetFiles(string pathOrContainerName)
            => _storage.GetFiles(pathOrContainerName);

        public bool HasFile(string pathOrContainerName, string fileName)
            => _storage.HasFile(pathOrContainerName, fileName);

        public async Task<List<FileUploadResponseDto>> UploadAsync(FileUploadDto fileUploadDto)
            => await _storage.UploadAsync(fileUploadDto);
    }
}
