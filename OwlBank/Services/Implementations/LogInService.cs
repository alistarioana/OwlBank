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

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepository;

    public LoginService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<string> Login(LoginRequest userRequest)
    {
        var user = await _userRepository.GetUserByEmail(userRequest.Email);
        if (user == null)
        {
            throw new Exception($"No user with email {userRequest.Email} registered.");
        }
        
        bool passwordValid = BCrypt.Net.BCrypt.Verify(userRequest.Password, user.Password);

        if (!passwordValid)
        {
            if (user.AccountLocketAt?.AddMinutes(30) <= DateTime.UtcNow)
            {
                user.LoginAttempt = 0;
                user.AccountLocketAt = null;
            }
            
            if (user.LoginAttempt >= 3)
            {
                user.AccountLocketAt = DateTime.UtcNow;
                
                await _userRepository.Update(user);
                throw new Exception($"Your account has been locked.");
            }
            
            user.LoginAttempt++;
            
            await _userRepository.Update(user);

            throw new AuthenticationException($"Wrong password. Attempt {user.LoginAttempt} / 3");
        }

        var claims = new List<Claim>
        {
            new Claim("User Id", user.ID.ToString()),
            new Claim("Email", user.Email)
        };

        foreach (var role in user.UserRoles)
        {
            claims.Add(new Claim("Roles", role.ToString()));
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
        DateOnly dateOnly = new DateOnly(userRequest.DateOfBirth.Value.Year, userRequest.DateOfBirth.Value.Month, userRequest.DateOfBirth.Value.Day);

        DateTime dateTime = dateOnly.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        
        User create = new UserBuilder()
                .SetFirstName(userRequest.FirstName)
                .SetLastName(userRequest.LastName)
                .SetEmail(userRequest.Email)
                .SetDateOfBirth(dateTime)
                .SetPhoneNumber(userRequest.PhoneNumber)
                .SetPassword(userRequest.Password)
                .SetConfirmationPassword(userRequest.Password)
                .Build();

        
        Random random = new Random();
        Card card = new Card();
        card.FirstName = userRequest.FirstName;
        card.LastName = userRequest.LastName;
        card.CardNumber = string.Concat(Enumerable.Range(0, 16)
            .Select(_ => random.Next(0, 10)));
        card.CVV = string.Concat(Enumerable.Range(0, 3)
            .Select(_ => random.Next(0, 9)));
        card.ExpirationDate = DateTime.UtcNow.AddYears(10);
        
        create.Cards.Add(card);
        
        await _userRepository.AddUser(create);
            
    }

    public async Task ResetPassword(string email, string password, string newpassword, string confirmPassword)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user == null)
        {
            throw new Exception($"No user with email: {email} found.");
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            throw new Exception("Invalid password");
        }

        if (newpassword != confirmPassword)
        {
            throw new Exception("Passwords does not match");
        }

        if (BCrypt.Net.BCrypt.Verify(newpassword,user.Password))
        {
            throw new Exception("Passwords cannot be set as the old password.");
        }
        user.Password =BCrypt.Net.BCrypt.HashPassword(newpassword);
        await _userRepository.Update(user);
    }
}