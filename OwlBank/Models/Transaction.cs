namespace OwlBank.Models;

public class Transaction
{
    public Guid Id { get; set; }

    public decimal? Amount { get; set; }

    public string? Description { get; set; }
    
    public string Type { get; set; }

    public DateTime? Date { get; set; }


}
