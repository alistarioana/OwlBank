using OwlBank.DTOs.UserDTO;

namespace OwlBank.Services;

public interface IUserService
{
    public Task DeleteUser(Guid id);
    public Task UpdateUser(Guid id, UpdateUserRequest userRequest);
    public Task Deposit(Guid id, decimal amount, string description);
    public Task Withdraw(Guid id, decimal amount, string description);
}