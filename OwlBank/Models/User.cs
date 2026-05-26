namespace OwlBank.Models;
public class User
{
    public Guid ID { get; set; } = Guid.NewGuid();
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal? Balance { get; set; }
    public List<BankStatement>? BankStatement { get; set; }
    public List<string> UserRoles { get; set; } = [nameof(Role.User)];
    public int LoginAttempt { get; set; }
    public DateTime? AccountLocketAt { get; set; }
    public List<Card> Cards { get; set; } = new List<Card>();
}  