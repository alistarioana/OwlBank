using OwlBank.DTOs.UserDTO;
using OwlBank.Models;

namespace OwlBank.Repository;

public interface IUserRepository
{
    public Task<User> AddUser(User user);
    public Task DeleteUser(Guid id);
    public Task UpdateUser(Guid id, UpdateUserRequest userRequest);
    public Task<User> GetUserById(Guid id);
    public Task<User?> GetUserByEmail(string email);
    public Task SaveChanges();
}