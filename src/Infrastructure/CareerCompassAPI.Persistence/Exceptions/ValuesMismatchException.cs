using System.Net;

namespace CareerCompassAPI.Persistence.Exceptions
{
    public class ValuesMismatchException : Exception, IBaseException
    {
        public int StatusCode { get; set; }
        public string CustomMessage { get; set; }
        public ValuesMismatchException(string message):base(message) 
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
            CustomMessage = message;
        }
    }
}
