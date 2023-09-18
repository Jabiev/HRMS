namespace CareerCompassAPI.Domain.Entities
{
    public class AppSetting
    {
        public Guid Id { get; set; }
        public string SettingName { get; set; }
        public string? SettingValue { get; set; }
    }
}
