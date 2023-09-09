namespace CareerCompassAPI.Application.DTOs.Response_DTOs
{
    public class ExceptionResponseDto
    {
        public int StatusCode { get; }
        public string CustomMessage { get; }

        public ExceptionResponseDto(int statusCode, string customMessage)
        {
            StatusCode = statusCode;
            CustomMessage = customMessage;
        }
    }
}
