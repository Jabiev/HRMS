using System.Net;

namespace CareerCompassAPI.Persistence.Exceptions.AuthExceptions
{
    public class SignInFailureException : Exception, IBaseException
    {
        public int StatusCode { get; set; }
        public string CustomMessage { get; set; }
        public SignInFailureException(string message):base(message) 
        {
            CustomMessage = message;
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
