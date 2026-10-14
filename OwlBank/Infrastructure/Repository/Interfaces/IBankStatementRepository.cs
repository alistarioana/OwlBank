using OwlBank.Models;

namespace OwlBank.Repository;


public interface IBankStatementRepository
{
    public Task SaveBankStatemant(BankStatement bankStatement);
    public Task<List<BankStatement>> GetStatementByDate(DateTime startDate, DateTime endDate,  Guid? userId);
    public Task<List<BankStatement>> ReceivedDetails(Guid userId, string name);

    public   Task<List<BankStatement>> GetAllStatements(Guid userId);
}