namespace OwlBank.Models;

public class Card
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string CVV { get; set; }
    public string CardNumber { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public bool IsBlocked { get; set; }
}