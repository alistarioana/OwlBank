using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using OwlBank.DTOs.UserDTO;
using OwlBank.Exceptions;

namespace OwlBank.Services;
using OwlBank.Repository;
using OwlBank.Models;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IBankStatementRepository _bankStatementRepository;

    public UserService(IUserRepository userRepository, IBankStatementRepository bankStatementRepository)
    {
        _userRepository = userRepository;
        _bankStatementRepository = bankStatementRepository;
    }

    public async Task DeleteUser(string id)
    {
        await _userRepository.DeleteUser(id);
    }

    public async Task UpdateUser(string id, UpdateUserRequest userRequest)
    {
        await _userRepository.UpdateUser(id, userRequest);
    }
    
    public async Task Deposit(string id, decimal amount,  string description)
    {
        var user = await _userRepository.GetUserById(id);

        if (user == null)
            throw new UserNotFoundException();

        if (amount <= 0)
            throw new Exception("Invalid amount");
        
        var timeStamp = DateTime.UtcNow;
        BankStatement bankStatement = new BankStatement();
        bankStatement.Description = description;
        bankStatement.ReceivedAmount = amount;
        bankStatement.TimeStamp = timeStamp;
        bankStatement.UserId = Guid.Parse(id);

        user.Balance += amount;

        await _userRepository.SaveChanges();
        await _bankStatementRepository.DepositAction(bankStatement);
    }
    
    public async Task Withdraw(string id, decimal amount, string description)
    {
        var user = await _userRepository.GetUserById(id);
        
        if (user == null) throw new UserNotFoundException();
        
        if (amount <= 0) throw new Exception("Invalid amount");

        if (user.Balance < amount)
            throw new Exception("Insufficient funds");
        
        var timeStamp = DateTime.UtcNow;
        BankStatement bankStatement = new BankStatement();
        bankStatement.Description = description;
        bankStatement.SpentAmount = amount;
        bankStatement.TimeStamp = timeStamp;
        bankStatement.UserId = Guid.Parse(id);;

        user.Balance -= amount;

        await _userRepository.SaveChanges();
        await _bankStatementRepository.WithdrawAction(bankStatement);
    }

    public async Task<List<BankStatement>> GetStatementByDateRange(string userId, DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate) throw new Exception("Start date must be earlier than end date.");
        
        return await _bankStatementRepository.GetStatementByDate(startDate, endDate, userId);
    }

    public async Task<decimal?> GetBalance(string id)
    {
        var user = await _userRepository.GetUserById(id);
        if(user == null) throw new UserNotFoundException();
        
        return user.Balance;
    }

    public async Task Transfer(string id, string phoneNumber, decimal amount)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null) throw new UserNotFoundException();
        if (amount <= 0) throw new Exception("Invalid amount");
        if (user.Balance < amount)
            throw new Exception("Insufficient funds");
        var receiverUser = await _userRepository.GetUserByPhoneNumber(phoneNumber);
        if (receiverUser == null) throw new UserNotFoundException();
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