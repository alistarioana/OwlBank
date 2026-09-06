using Microsoft.EntityFrameworkCore;
using OwlBank.Models;

namespace OwlBank.Repository;

[Dependency(typeof(ICardRepository))]

public class CardRepository: ICardRepository
{ 
    private readonly OwlBankDBContext _dbContext;
    
    public CardRepository(OwlBankDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChanges()
    {
       await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Card>> GetCards()
    {
        return await _dbContext.Cards.ToListAsync();
    }
}