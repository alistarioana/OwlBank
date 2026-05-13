namespace OwlBank.Models;

public class BankStatement
{
    public Guid Id { get; set; } 
    public Guid? UserId { get; set; }
    public User User { get; set; }
    public DateTime? TimeStamp { get; set; }
    public decimal? SpentAmount { get; set; }
    public decimal? ReceivedAmount { get; set; }
    public string? Description { get; set; }
    
}