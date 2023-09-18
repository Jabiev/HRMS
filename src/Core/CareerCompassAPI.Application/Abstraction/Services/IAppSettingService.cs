using CareerCompassAPI.Application.DTOs.AppSetting_DTOs;

namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IAppSettingService
    {
       Task UpdateSettingValue(AppSettingMessageUpdateDto update);
    }
}
