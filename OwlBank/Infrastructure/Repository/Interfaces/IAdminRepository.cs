using OwlBank.Models;

namespace OwlBank.Repository;

public interface IAdminRepository
{
    public Task<List<User>> GetUsers();
    public Task<User?> FindUserById(string id);
    public Task SaveChanges();
    public Task DeleteUser(string id);
}