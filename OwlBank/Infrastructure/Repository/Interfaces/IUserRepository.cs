using OwlBank.DTOs.UserDTO;

namespace OwlBank.Repository;
using OwlBank.Models;

public interface IUserRepository
{
    public Task<User> AddUser(User user);
    public Task DeleteUser(string id);
    public Task UpdateUser(string id, UpdateUserRequest userRequest);
    public Task<User?> GetUserById(string id);
    public Task<User?> GetUserByEmail(string email);
    public Task SaveChanges();
    public Task<User?> GetUserByPhoneNumber(string phoneNumber);
    public Task Update(User user);
    public Task<Card> AddCard(Card card);
    public Task DeleteCard(Card card);
}