using OwlBank.Models;

namespace OwlBank.Repository;

public interface ICardRepository
{
    public Task SaveChanges();
    
    public Task<List<Card>> GetCards();
}