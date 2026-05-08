namespace OwlBank.DTOs.UserDTO;

public class UserResponse
{
    public Guid? ID { get; set; } = Guid.NewGuid();
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
}