using Microsoft.EntityFrameworkCore;
using OwlBank.Models;

namespace OwlBank.Repository;

[Dependency(typeof(IBankStatementRepository))]

public class BankStatementRepository : IBankStatementRepository
{
    private readonly OwlBankDBContext _context;
    public BankStatementRepository(OwlBankDBContext context)
    {
        _context = context;
    }
    
    public async Task SaveBankStatemant(BankStatement bankStatement)
    { 
        _context.BankStatement.Add(bankStatement);
        await _context.SaveChangesAsync();
    }

    public async Task<List<BankStatement>> GetStatementByDate(DateTime startDate, DateTime endDate, Guid? userId)
    {
        startDate = DateTime.SpecifyKind(
            startDate.Date,
            DateTimeKind.Utc);

        endDate = DateTime.SpecifyKind(
            endDate.Date.AddDays(1).AddTicks(-1),
            DateTimeKind.Utc);

        var statement = _context.BankStatement
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.TimeStamp >= startDate && x.TimeStamp <= endDate);
        
        return statement.ToList();
    }

    public async Task<List<BankStatement>> ReceivedDetails(Guid userId, string name)
    {
       return _context.BankStatement
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Where(x => x.Description.ToLower().Contains(name.ToLower())).ToList();
    }

    public async Task<List<BankStatement>> GetAllStatements(Guid userId)
    {
        return _context.BankStatement
            .AsNoTracking()
            .Where(x => x.UserId == userId).ToList();
    }
}