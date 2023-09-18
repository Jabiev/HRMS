using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.AppSetting_DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompassAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {
        private readonly IAppSettingService _settingService;

        public AppSettingsController(IAppSettingService settingService)
        {
            _settingService = settingService;
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateValue(AppSettingMessageUpdateDto appSettingMessageUpdateDto)
        {
            await _settingService.UpdateSettingValue(appSettingMessageUpdateDto);
            return Ok();
        }
    }
}
