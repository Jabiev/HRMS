using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.Abstraction.Storage.Azure;
using CareerCompassAPI.Application.DTOs.File_DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CareerCompassAPI.Infrastructure.Services.Azure
{
    public class AzureStorage : IAzureStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IFileService _fileService;
        BlobContainerClient _blobContainerClient;
        public AzureStorage(IConfiguration configuration, IFileService fileService)
        {
            _blobServiceClient = new(configuration["Storage:Azure"]);
            _fileService = fileService;
        }
        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(b=>b.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(FileUploadDto fileUploadDto)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(fileUploadDto.containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<(string fileName, string pathOrContainerName)> datas = new();

            foreach (IFormFile file in fileUploadDto.files)
            {
                string fileName = file.FileName;
                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);

                await blobClient.UploadAsync(file.OpenReadStream(), overwrite: true);
                datas.Add((fileName, fileUploadDto.containerName));

                FileCreateDto fileCreateDto = new(
                    fileName,
                    blobClient.Uri.AbsoluteUri,
                    fileUploadDto.containerName,
                    file.ContentType,
                    file.Length,
                    fileUploadDto.appUserId
                );

                await _fileService.CreateAsync(fileCreateDto);
            }
            return datas;
        }
    }
}
