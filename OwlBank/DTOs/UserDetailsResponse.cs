namespace OwlBank.DTOs.UserDTO;

public class UserDetailsResponse
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public decimal? Balance { get; set; }
    public List<CardResponse>? Cards { get; set; } = [];

}