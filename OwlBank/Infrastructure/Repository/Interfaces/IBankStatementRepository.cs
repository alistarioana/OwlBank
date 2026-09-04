using OwlBank.Models;

namespace OwlBank.Repository;


public interface IBankStatementRepository
{
    public Task DepositAction(BankStatement bankStatement);
    public Task TransferAction(BankStatement bankStatement);
    public Task WithdrawAction(BankStatement bankStatement);
    public Task<List<BankStatement>> GetStatementByDate(DateTime startDate, DateTime endDate,  string? userId);
    public Task<List<BankStatement>> ReceivedDetails(string userId, string name);

    public   Task<List<BankStatement>> GetAllStatements(string userId);
}