namespace OwlBank.Services;
using OwlBank.Models;
using OwlBank.DTOs.UserDTO;
public interface IAdminService
{
    public Task<List<UserResponse>> GetUsers();
}