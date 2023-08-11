using System.Net;

namespace CareerCompassAPI.Persistence.Exceptions.AuthExceptions
{
    public class UserRegistrationException : Exception, IBaseException
    {
        public int StatusCode { get; set; }
        public string CustomMessage { get; set; }
        public UserRegistrationException(string message) : base(message)
        {
            CustomMessage = message;
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
