using OwlBank.Repository;
using OwlBank.Services.Interfaces;
using System.Security.Cryptography;

namespace OwlBank.Services.Implementations
{
    [Dependency(typeof(IPasswordHasher))]
    public class PasswordHasher: IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int keySize = 32;
        private const int Iterations = 100000;
        private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA256;

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                _hashAlgorithm,
                keySize
            );

            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('-');

            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);
            byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                _hashAlgorithm,
                keySize
            );
            return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);    
        }
    }
}
