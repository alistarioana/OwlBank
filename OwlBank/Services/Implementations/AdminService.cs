using OwlBank.Exceptions;

namespace OwlBank.Services;

using OwlBank.DTOs.UserDTO;
using OwlBank.Repository;
using OwlBank.Models;
using OwlBank.Services.Interfaces;

[Dependency(typeof(IAdminService))]
public class AdminService : IAdminService
{
        private readonly IAdminRepository _db;
        private readonly IPasswordHasher _passwordHasher;

    public AdminService(IAdminRepository db, IPasswordHasher passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;   
        }

        public async Task<List<UserResponse>> GetUsers()
        {
            var getUsers = await _db.GetUsers();
            
            return getUsers.Select(x => new UserResponse
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = DateOnly.FromDateTime(x.DateOfBirth.Value),
                Email = x.Email,
                PhoneNumber =  x.PhoneNumber,
                Password = x.Password,
                ID =  x.ID,
                Roles = x.UserRoles,
                Cards = x.Cards.Select(y=> new CardResponse
                {
                    FirstName = y.FirstName,
                    LastName = y.LastName,
                    LastFourDigitsCardNumber = GetLastFourDigits(y)
                }).ToList()
            }).ToList();
        }

        private string GetLastFourDigits(Card card)
        {
            var cardNumber = card.CardNumber.ToCharArray();
            for (int i = 0; i < cardNumber.Length; i++)
            {
                if (cardNumber.Length - 4 == i)
                {
                    break;
                }
                cardNumber[i]= '*';
            }
            
            return new string(cardNumber);
        }

        public async Task UpdateUserRole(Guid id, List<string> roles)
        {
            var user = await _db.FindUserById(id);
            if (user == null)
            {
                throw new ProblemException("User not found", "Unable to find user.");
            }

            foreach (var role in roles)
            {
                if (!user.UserRoles.Contains(role))
                {
                    user.UserRoles.Add(role);
                }
            }

            await _db.SaveChanges();
        }
        
        public async Task ResetPassword(Guid id)
        {
            var user = await _db.FindUserById(id);
            if (user == null)
            {
                throw new ProblemException("User not found", "The specified user could not be found.");
            }
            user.Password = _passwordHasher.HashPassword("OwlBank@123");

            await _db.SaveChanges();
        }

        public async Task DeleteUser(Guid id)
        {
            await _db.DeleteUser(id);
        }
}