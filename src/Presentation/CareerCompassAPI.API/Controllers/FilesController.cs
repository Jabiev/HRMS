using CareerCompassAPI.Application.Abstraction.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IAzureStorage _azureStorage;

        public FilesController(IAzureStorage azureStorage)
        {
            _azureStorage = azureStorage;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromForm] string containerName, [FromForm] IFormFileCollection files)
        {
            var result = await _azureStorage.UploadAsync(containerName,files);
            return Ok(result);
        }
    }
}
