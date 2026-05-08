namespace OwlBank.Services;
using OwlBank.DTOs.UserDTO;
public interface IAdminService
{
    public Task<List<UserResponse>> GetUsers();
}