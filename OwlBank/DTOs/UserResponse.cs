using OwlBank.Models;

namespace OwlBank.DTOs.UserDTO;

public class UserResponse
{
    public Guid? ID { get; set; } = Guid.NewGuid();
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public List<string> Roles { get; set; }
    public List<CardResponse> Cards { get; set; }
    
}