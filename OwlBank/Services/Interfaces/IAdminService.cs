using OwlBank.DTOs.UserDTO;

namespace OwlBank.Services;
public interface IAdminService
{
    public Task<List<UserResponse>> GetUsers();
}