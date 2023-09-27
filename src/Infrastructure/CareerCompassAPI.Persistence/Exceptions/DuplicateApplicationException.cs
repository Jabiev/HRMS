using System.Net;

namespace CareerCompassAPI.Persistence.Exceptions
{
    public class DuplicateApplicationException : Exception, IBaseException
    {
        public int StatusCode { get; set; }
        public string CustomMessage { get; set; }
        public DuplicateApplicationException(string message) : base(message)
        {
            CustomMessage = message;
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
