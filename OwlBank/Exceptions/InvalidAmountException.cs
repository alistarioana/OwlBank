namespace OwlBank.Exceptions;

public class InvalidAmountException: Exception
{
    public InvalidAmountException() : base("Invalid amount"){}
}