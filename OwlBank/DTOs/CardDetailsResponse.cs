namespace OwlBank.DTOs.UserDTO;

public class CardDetailsResponse
{
    public string CVV { get; set; }
    public string ExpirationDate { get; set; }
    public string CardNumber { get; set; }
    public bool IsBlocked { get; set; }
}