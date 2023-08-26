namespace CareerCompassAPI.Application.DTOs.File_DTOs
{
    public record FileCreateDto(string name, string blobPath, string containerName, string contentType, long size, Guid appUserId);
}
