using OwlBank.Models;

namespace OwlBank.Repository;


public interface IBankStatementRepository
{
    public Task DepositAction(BankStatement bankStatement);
    public Task WithdrawAction(BankStatement bankStatement);
    
    public Task<List<BankStatement>> GetStatementByDate(DateTime startDate, DateTime endDate,  string? userId);
    
    
}