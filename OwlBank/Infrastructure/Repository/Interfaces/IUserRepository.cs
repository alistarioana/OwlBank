using OwlBank.DTOs.UserDTO;

namespace OwlBank.Repository;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OwlBank.Models;

public interface IUserRepository
{
    public Task<User> AddUser(User user);
    public Task DeleteUser(Guid id);
    public Task UpdateUser(Guid id, UpdateUserRequest userRequest);
    public Task<User?> GetUserById(Guid id);
    public Task<User?> GetUserByEmail(string email);
    public Task SaveChanges();
    public Task<User?> GetUserByPhoneNumber(string phoneNumber);
    public Task Update(User user);
    public Task<Card> AddCard(Card card);
    public Task DeleteCard(Card card);

    public ChangeTracker GetChangeTracker();
}