using Microsoft.AspNetCore.Http;

namespace CareerCompassAPI.Application.DTOs.File_DTOs
{
    public record FileUploadDto(string containerName, IFormFileCollection files, string appUserId);
}
