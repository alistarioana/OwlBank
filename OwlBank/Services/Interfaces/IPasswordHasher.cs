using OwlBank.Repository;

namespace OwlBank.Services.Interfaces
{

    public interface IPasswordHasher
    {
        public string HashPassword(string password);    
        public bool VerifyPassword(string password, string hashedPassword);
    }
}
