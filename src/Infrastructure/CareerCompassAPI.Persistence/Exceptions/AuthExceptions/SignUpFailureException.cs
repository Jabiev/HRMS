using System.Net;

namespace CareerCompassAPI.Persistence.Exceptions.AuthExceptions
{
    public class SignUpFailureException : Exception, IBaseException
    {
        public int StatusCode { get; set; }
        public string CustomMessage { get; set; }
        public SignUpFailureException(string message) : base(message)
        {
            CustomMessage = message;
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
