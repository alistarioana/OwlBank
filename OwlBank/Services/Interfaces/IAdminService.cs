namespace OwlBank.Services;
using OwlBank.Models;
using OwlBank.DTOs.UserDTO;
public interface IAdminService
{
    public Task<List<UserResponse>> GetUsers();
    public Task UpdateUserRole(Guid id, List<string> roles);
    public Task UpdatePassword(Guid id);
    public Task DeleteUser(Guid id);
}