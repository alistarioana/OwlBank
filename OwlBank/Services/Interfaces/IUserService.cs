using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OwlBank.DTOs.UserDTO;
using OwlBank.Models;

namespace OwlBank.Services;

public interface IUserService
{
    public Task DeleteUser(Guid id);
    public Task UpdateUser(Guid id, UpdateUserRequest userRequest);
    public Task Deposit(Guid id, decimal amount, string description);
    public Task Withdraw(Guid id, decimal amount, string description);
    public Task<List<BankStatement>> GetStatementByDateRange(Guid userId, DateTime startDate, DateTime endDate);
    public Task<decimal> GetBalance(Guid id);
    public Task Transfer(Guid id, string phoneNumber, decimal amount);
    public Task ResetPassword(string email, string password, string newpassword, string confirmPassword);
    public Task<UserDetailsResponse> GetUserDetails(Guid userId);
    public Task<List<BankStatement>> TransferDetails(Guid userId, string name);
    public Task<ContactDetailsResponse> GetContactDetails(Guid id);
    public Task<CardDetailsResponse> ShowCardDetails(Guid id, string password, string cardID);
     
    public Task<AddCardsResponse> AddCard(Guid userId);
    public Task DeleteCard(string cardId, Guid userId);
    public Task BlockCard(string cardId, Guid userId);
    public Task ActivateCard(string cardId, Guid userId);

    public Task<List<Transaction>> GetTransactionsAsync(Guid userId);

    public Task<List<Card>> GetAllCardsAsync(Guid userId);
    public Task<string> RegenerateTokens(string refreshToken);
}