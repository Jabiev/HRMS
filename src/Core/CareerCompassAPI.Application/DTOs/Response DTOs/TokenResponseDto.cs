namespace CareerCompassAPI.Application.DTOs.Response_DTOs
{
    public record TokenResponseDto(string token, DateTime jwtExpiration, string refreshToken, DateTime refreshTokenExpiration);
}
