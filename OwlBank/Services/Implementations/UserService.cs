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

    public async Task<UserDetailsResponse> GetUserDetails(string userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null) throw new UserNotFoundException();

        return new UserDetailsResponse()
        {
            Balance = user.Balance,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Cards = user.Cards.Select(x => new CardResponse
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                LastFourDigitsCardNumber = MapCardNumber(x.CardNumber)
            }).ToList()
        };
    }

    private string MapCardNumber(string card)
    {
        char[] result = card.ToCharArray();
        
        for (int i = 0; i < card.Length-4; i++)
        {
            result[i] = '*';
        }
        
        return new string(result);
    }

    public async Task<List<BankStatement>> TransferDetails(string userId, string name)
    {
       return await _bankStatementRepository.ReceivedDetails(userId, name);
    }

    public async Task<ContactDetailsResponse> GetContactDetails(string id)
    {
        var user = await _userRepository.GetUserById(id);
        ContactDetailsResponse contactDetails = new ContactDetailsResponse();
        contactDetails.Email = user.Email;
        contactDetails.PhoneNumber = user.PhoneNumber; 
        return contactDetails;
    }

    public async Task<CardDetailsResponse> ShowCardDetails(string id, string password, string cardID)
    {
        var user = await _userRepository.GetUserById(id);
        if (!BCrypt.Net.BCrypt.Verify(password,user.Password))
        {
            throw new Exception("Invalid password.");
        }
        
        CardDetailsResponse card = new CardDetailsResponse();
        var userCard = user.Cards?.Where(x => x.Id.ToString() == cardID).FirstOrDefault();
        if (userCard == null)
        {
            throw new Exception("Card not found.");
        }
        
        card.CardNumber = userCard.CardNumber;
        card.CVV = userCard.CVV;
        card.ExpirationDate = DateOnly.FromDateTime(userCard.ExpirationDate).ToString("MM/yyyy");

        var expire = card.ExpirationDate.Split("/");
        
        expire[1] = expire[1].Substring(2);
        
        card.ExpirationDate = string.Join("/", expire); 
        
        return card;
    }

    public async Task<AddCardsResponse> AddCard(string userId)
    {
        var user = await _userRepository.GetUserById(userId);
        
        Card card = new Card();
        Random random = new Random();
        card.FirstName = user.FirstName;
        card.LastName = user.LastName;
        card.CardNumber = string.Concat(Enumerable.Range(0, 16)
            .Select(_ => random.Next(0, 10)));
        card.CVV = string.Concat(Enumerable.Range(0, 3)
            .Select(_ => random.Next(0, 9)));
        card.ExpirationDate = DateTime.UtcNow.AddYears(10);
        card.UserId = user.ID;
        card.User = user;
        user.Cards.Add(card);
        await _userRepository.AddCard(card);
        await _userRepository.SaveChanges();
        
        AddCardsResponse cardResponse = new AddCardsResponse();
        cardResponse.CardNumber = card.CardNumber;
        cardResponse.ExpirationDate = DateOnly.FromDateTime(card.ExpirationDate).ToString("MM/yyyy");

        var expire = cardResponse.ExpirationDate.Split("/");
        
        expire[1] = expire[1].Substring(2);
        
        cardResponse.ExpirationDate = string.Join("/", expire); 
        cardResponse.CVV = card.CVV;
        cardResponse.Name = card.FirstName + " " + card.LastName;
        
        return cardResponse;
    }
}