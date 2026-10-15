using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OwlBank.Auth.Contracts.DTOs;
using OwlBank.Auth.Contracts.Interfaces;
using OwlBank.Exceptions;
using OwlBank.Models;
using OwlBank.Repository;
using OwlBank.Services.Interfaces;

namespace OwlBank.Auth.Services;

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenCookieService _refreshTokenCookieService;

    public LoginService(IUserRepository userRepository, IPasswordHasher passwordHasher, IRefreshTokenCookieService refreshTokenCookieService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _refreshTokenCookieService = refreshTokenCookieService;
    }
    public async Task<string> Login(LoginRequest userRequest)
    {
        var user = await _userRepository.GetUserByEmail(userRequest.Email);

        if (user == null)
        {
            throw new ProblemException("User not Found", $"No user with email {userRequest.Email} registered.");
        }

        bool passwordValid = _passwordHasher.VerifyPassword(userRequest.Password, user.Password);


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
                throw new ProblemException($"Your account has been locked.", $"Your account has been locked due to multiple failed login attempts.");
            }

            user.LoginAttempt++;

            await _userRepository.Update(user);

            throw new ProblemException("Wrong Password", $"Wrong password. Attempt {user.LoginAttempt} / 3");
        }

        var claims = new List<Claim>
        {
            new Claim("User Id", user.ID.ToString()),
            new Claim("Email", user.Email)

        };

        var jwt = _refreshTokenCookieService.GenerateToken(user);

        _refreshTokenCookieService.SetRefreshToken(user);

        await _userRepository.SaveChangesAsync();

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
        create.Password = _passwordHasher.HashPassword(create.Password);    
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

        if (!_passwordHasher.VerifyPassword(password, user.Password))
        {
            throw new Exception("Invalid password");
        }

        if (newpassword != confirmPassword)
        {
            throw new Exception("Passwords does not match");
        }

        if (_passwordHasher.VerifyPassword(newpassword, user.Password))
        {
            throw new Exception("Passwords cannot be set as the old password.");
        }
        user.Password = _passwordHasher.HashPassword(newpassword);
        await _userRepository.Update(user);
    }
}
