namespace OwlBank.DTOs.UserDTO;

public class WithdrawBalanceRequest
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
}