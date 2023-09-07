using CareerCompassAPI.Application.Abstraction.Storage.Azure;
using CareerCompassAPI.Application.DTOs.File_DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IAzureStorage _azureStorage;
        private readonly ILogger<FilesController> _logger;
        public FilesController(IAzureStorage azureStorage, ILogger<FilesController> logger)
        {
            _azureStorage = azureStorage;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromForm] FileUploadDto fileUploadDto)
        {
            var result = await _azureStorage.UploadAsync(fileUploadDto);
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
