using OwlBank.Exceptions;

namespace OwlBank.Services;
using OwlBank.DTOs.UserDTO;
using OwlBank.Repository;
using OwlBank.Models;

[Dependency(typeof(IAdminService))]
public class AdminService : IAdminService
{
        private readonly IAdminRepository _db;

        public AdminService(IAdminRepository db)
        {
            _db = db;
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

        public async Task UpdateUserRole(string id, List<string> roles)
        {
            var user = await _db.FindUserById(id);
            if (user == null)
            {
                throw new UserNotFoundException();
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
        
        public async Task UpdatePassword(string id)
        {
            var user = await _db.FindUserById(id);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword("OwlBank2026");
            await _db.SaveChanges();

        }

        public async Task DeleteUser(string id)
        {
            await _db.DeleteUser(id);
        }
}