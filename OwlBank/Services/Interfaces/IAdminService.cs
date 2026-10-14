namespace OwlBank.Services;
using OwlBank.Models;
using OwlBank.DTOs.UserDTO;
public interface IAdminService
{
    public Task<List<UserResponse>> GetUsers();
    public Task UpdateUserRole(Guid id, List<string> roles);
    public Task ResetPassword(Guid id);
    public Task DeleteUser(Guid id);
}