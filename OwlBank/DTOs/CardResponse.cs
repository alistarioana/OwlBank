namespace OwlBank.DTOs.UserDTO;

public class CardResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string LastFourDigitsCardNumber { get; set; }
    public Guid CardId { get; set; }
    public bool IsBlocked { get; set; }
}