using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using OwlBank.DTOs.UserDTO;
using OwlBank.Models;
using OwlBank.Repository;

namespace OwlBank.Services;

public class LogInService : ILogInService
{
    private readonly IUserRepository _userRepository;

    public LogInService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<string> Login(LoginRequest userRequest)
    {
        var user = await _userRepository.GetUserByEmail(userRequest.Email);
        if (user == null)
        {
            throw new Exception($"No User with given email: {user.Email} registered.");
        }
        
        if (!BCrypt.Net.BCrypt.Verify(userRequest.Password, user.Password))
        {
            throw new AuthenticationException("Invalid password");
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        foreach (var role in user.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("rkjlngbaekj-jRNVWKrnb-ekfrjnvoern"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    public async Task Register(CreateUserRequest userRequest)
    {
        if (await _userRepository.GetUserByEmail(userRequest.Email) != null)
        {
            throw new Exception("Email already exists");
        }
        
        User create = new UserBuilder()
                .SetFirstName(userRequest.FirstName)
                .SetLastName(userRequest.LastName)
                .SetBalance(userRequest.Balance)
                .SetEmail(userRequest.Email)
                .SetDateOfBirth(userRequest.DateOfBirth)
                .SetPhoneNumber(userRequest.PhoneNumber)
                .SetPassword(userRequest.Password)
                .SetUserName(userRequest.Username)
                .Build();

            await _userRepository.AddUser(create);
    }
}