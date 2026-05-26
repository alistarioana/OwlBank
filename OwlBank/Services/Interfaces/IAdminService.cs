namespace OwlBank.Services;
using OwlBank.Models;
using OwlBank.DTOs.UserDTO;
public interface IAdminService
{
    public Task<List<UserResponse>> GetUsers();
    public Task UpdateUserRole(string id, List<string> roles);
    public Task UpdatePassword(string id);
    public Task DeleteUser(string id);
}