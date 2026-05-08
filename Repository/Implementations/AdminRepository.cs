using Microsoft.EntityFrameworkCore;
using OwlBank.Models;

namespace OwlBank.Repository;

public class AdminRepository : IAdminRepository
{
    private readonly OwlBankDBContext _dbContext;
    
    public AdminRepository(OwlBankDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<User>> GetUsers()
    {
        return await _dbContext.Users.ToListAsync();
    }
}