using OwlBank.Models;

namespace OwlBank.Repository;

public class BankStatementRepository : IBankStatementRepository
{
    private readonly OwlBankDBContext _context;
    public BankStatementRepository(OwlBankDBContext context)
    {
        _context = context;
    }
    
    public Task DepositAction(BankStatement bankStatement)
    { 
        _context.BankStatement.Add(bankStatement);
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task WithdrawAction(BankStatement bankStatement)
    {
        _context.BankStatement.Add(bankStatement);
        _context.SaveChanges();
        return Task.CompletedTask;
    }
}