using OwlBank.Models;

namespace OwlBank.Repository;

public interface IAdminRepository
{
    public Task<List<User>> GetUsers();
    public Task<User?> FindUserById(Guid id);
    public Task SaveChanges();
    public Task DeleteUser(Guid id);
}