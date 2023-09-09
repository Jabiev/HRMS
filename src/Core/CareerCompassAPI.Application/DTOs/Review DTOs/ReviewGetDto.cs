namespace CareerCompassAPI.Application.DTOs.Review_DTOs
{
    public record ReviewGetDto(string firstName, string lastName,string title, string description, decimal rating);
}
