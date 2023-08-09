using System.Net;

namespace CareerCompassAPI.Persistence.Exceptions
{
    public interface IBaseException
    {
        public int StatusCode { get; set; }
        public string CustomMessage { get; set; }
    }
}
