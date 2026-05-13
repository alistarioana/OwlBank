using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OwlBank.DTOs.UserDTO;
using OwlBank.Models;

namespace OwlBank.Services;

public interface IUserService
{
    public Task DeleteUser(string id);
    public Task UpdateUser(string id, UpdateUserRequest userRequest);
    public Task Deposit(string id, decimal amount, string description);
    public Task Withdraw(string id, decimal amount, string description);
    public Task<List<BankStatement>> GetStatementByDateRange(string userId, DateTime startDate, DateTime endDate);
    public Task<decimal?> GetBalance(string id);
    public Task Transfer(string id, string phoneNumber, decimal amount);
}