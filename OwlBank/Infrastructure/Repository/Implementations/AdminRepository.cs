using Microsoft.EntityFrameworkCore;
using OwlBank.Exceptions;

namespace OwlBank.Repository;
using OwlBank.Models;

[Dependency(typeof(IAdminRepository))]
public class AdminRepository : IAdminRepository
{
    private readonly OwlBankDBContext _dbContext;
    
    public AdminRepository(OwlBankDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<User>> GetUsers()
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(x => x.Cards)
            .ToListAsync();
    }

    public async Task<User?> FindUserById(Guid id)
    {
        return await _dbContext.Users
            .Where(x => x.ID == id)
            .FirstOrDefaultAsync();
    }

    public async Task SaveChanges()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteUser(Guid id)
    {
        var user = await _dbContext.Users.Where(x => x.ID == id).FirstOrDefaultAsync();
        
        if (user == null) throw new ProblemException("User not found", "Unable to find user.");
        
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}