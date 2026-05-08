using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OwlBank.DTOs.UserDTO;
using OwlBank.Models;

namespace OwlBank.Services;

public interface IUserService
{
    public Task AddUser(CreateUserRequest userRequest);
    public Task DeleteUser(Guid id);
    public Task UpdateUser(Guid id, UpdateUserRequest userRequest);
    public Task Deposit(Guid id, decimal amount, string description);
    public Task Withdraw(Guid id, decimal amount, string description);
    public Task<string> Login(LoginRequest userRequest);
}