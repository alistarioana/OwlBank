using OwlBank.DTOs.UserDTO;
using OwlBank.Repository;

namespace OwlBank.Services;

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
                Username = x.Username,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                Email = x.Email,
                PhoneNumber =  x.PhoneNumber,
                Password = x.Password,
                ID =  x.ID
            }).ToList();
        }
}