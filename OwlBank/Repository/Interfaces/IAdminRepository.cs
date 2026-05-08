using OwlBank.Models;

namespace OwlBank.Repository;

public interface IAdminRepository
{
    public Task<List<User>> GetUsers();
}