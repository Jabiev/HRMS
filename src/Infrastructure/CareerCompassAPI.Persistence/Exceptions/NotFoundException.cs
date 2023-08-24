using System.Net;

namespace CareerCompassAPI.Persistence.Exceptions
{
    public class NotFoundException : Exception, IBaseException
    {
        public int StatusCode { get; set; }
        public string CustomMessage { get; set; }
        public NotFoundException(string message):base(message) 
        {
            CustomMessage = message;
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
