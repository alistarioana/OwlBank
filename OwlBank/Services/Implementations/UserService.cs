using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using OwlBank.DTOs.UserDTO;
using OwlBank.Exceptions;
using OwlBank.Models;
using OwlBank.Repository;

namespace OwlBank.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IBankStatementRepository _bankStatementRepository;

    public UserService(IUserRepository userRepository, IBankStatementRepository bankStatementRepository)
    {
        _userRepository = userRepository;
        _bankStatementRepository = bankStatementRepository;
    }

    public async Task DeleteUser(Guid id)
    {
        await _userRepository.DeleteUser(id);
    }

    public async Task UpdateUser(Guid id, UpdateUserRequest userRequest)
    {
        await _userRepository.UpdateUser(id, userRequest);
    }
    
    public async Task Deposit(Guid id, decimal amount,  string description)
    {
        var user = await _userRepository.GetUserById(id);

        if (user == null)
            throw new UserNotFoundException();

        if (amount <= 0)
            throw new InvalidAmountException();
        
        var timeStamp = DateTime.UtcNow;
        BankStatement bankStatement = new BankStatement();
        bankStatement.Description = description;
        bankStatement.ReceivedAmount = amount;
        bankStatement.TimeStamp = timeStamp;
        bankStatement.UserId = id;

        user.Balance += amount;

        await _userRepository.SaveChanges();
        await _bankStatementRepository.DepositAction(bankStatement);
    }
    
    public async Task Withdraw(Guid id, decimal amount, string description)
    {
        var user = await _userRepository.GetUserById(id);
        
        if (user == null) throw new Exception("User not found");

        if (amount <= 0) throw new Exception("Invalid amount");

        if (user.Balance < amount)
            throw new Exception("Insufficient funds");
        
        var timeStamp = DateTime.UtcNow;
        BankStatement bankStatement = new BankStatement();
        bankStatement.Description = description;
        bankStatement.SpentAmount = amount;
        bankStatement.TimeStamp = timeStamp;
        bankStatement.UserId = id;

        user.Balance -= amount;

        await _userRepository.SaveChanges();
        await _bankStatementRepository.WithdrawAction(bankStatement);
    }

    public async Task<string> Login(LoginRequest userRequest)
    {
       var user = await _userRepository.GetUserByEmail(userRequest.Email);
       if (!BCrypt.Net.BCrypt.Verify(userRequest.Password, user.Password))
       {
           throw new AuthenticationException("Invalid password");
       }
       var claims = new[]
       {
           new Claim(ClaimTypes.Name, user.Username),
           new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
       };

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
}