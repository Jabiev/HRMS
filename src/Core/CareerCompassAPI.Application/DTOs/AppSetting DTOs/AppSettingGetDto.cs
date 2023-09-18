namespace CareerCompassAPI.Application.DTOs.AppSetting_DTOs
{
    public record AppSettingGetDto(Guid settingId, string settingName, string settingValue);
}
