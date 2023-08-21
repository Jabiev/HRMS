using CareerCompassAPI.Application.Abstraction.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            var result = await _azureStorage.UploadAsync(containerName, files);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult ListFiles(string containerName)
        {
            var files = _azureStorage.GetFiles(containerName);
            return Ok(files);
        }

        [HttpGet("[action]")]
        public IActionResult FileExists(string containerName, string fileName)
        {
            var exists = _azureStorage.HasFile(containerName, fileName);
            return Ok(exists);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(string containerName, string fileName)
        {
            await _azureStorage.DeleteAsync(containerName, fileName);
            return Ok();
        }
    }
}
