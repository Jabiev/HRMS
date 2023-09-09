using System.Net;

namespace CareerCompassAPI.Persistence.Exceptions
{
    public class LimitExceededException : Exception, IBaseException
    {
        public int StatusCode { get; set; }
        public string CustomMessage { get; set; }
        public LimitExceededException(string message):base(message) 
        {
            CustomMessage = message;
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
