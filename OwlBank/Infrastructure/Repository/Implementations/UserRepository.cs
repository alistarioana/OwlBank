using Microsoft.EntityFrameworkCore;
using OwlBank.DTOs.UserDTO;
using OwlBank.Models;

namespace OwlBank.Repository;

public class UserRepository : IUserRepository
{
    private readonly OwlBankDBContext _dbContext;
    public UserRepository(OwlBankDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddUser(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var addUser = await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return addUser.Entity;
    }

    public async Task DeleteUser(string? id)
    {
        var removeUser = _dbContext.Users.Remove(_dbContext.Users.Find(id));
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUser(string? id, UpdateUserRequest userRequest)
    {
        var updateUser = await _dbContext.Users.FindAsync(id);
        if (updateUser == null) return;
        
        if (userRequest.FirstName != null) updateUser.FirstName = userRequest.FirstName;
        if (userRequest.LastName != null) updateUser.LastName = userRequest.LastName;
        if (userRequest.Email != null) updateUser.Email = userRequest.Email;
        if (userRequest.Password != null) updateUser.Password = userRequest.Password;
        if (userRequest.PhoneNumber != null) updateUser.PhoneNumber = userRequest.PhoneNumber;
        
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<User?> GetUserById(string id)
    {
        return await _dbContext.Users.Include(u => u.Cards).FirstOrDefaultAsync(u => u.ID.ToString() == id);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
       
        return user;
    }

    public async Task SaveChanges()
    {
        await _dbContext.SaveChangesAsync();
    }

    public Task<User?> GetUserByPhoneNumber(string phoneNumber)
    {
        var user =_dbContext.Users.Where(x => x.PhoneNumber == phoneNumber);
        return user.FirstOrDefaultAsync();
    }
    
    public async Task Update(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<Card> AddCard(Card card)
    {   
        var addCard = await _dbContext.Cards.AddAsync(card);
        await _dbContext.SaveChangesAsync();
        return addCard.Entity;
    }

    public async Task DeleteCard(Card card)
    {
        _dbContext.Cards.Remove(card);
        await _dbContext.SaveChangesAsync();
    }
}