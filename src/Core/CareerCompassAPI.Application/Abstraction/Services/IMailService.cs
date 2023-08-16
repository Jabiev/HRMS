namespace CareerCompassAPI.Application.Abstraction.Services
{
    public interface IMailService
    {
        void SendEmail(Message message);
    }
}
