using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OwlBank.DTOs.UserDTO;
using OwlBank.Models;

namespace OwlBank.Controllers;
using OwlBank.Services;

[ApiController]
[Route("users")]
[Authorize(Roles = nameof(Role.User))]
public class UserController : ControllerBase
{
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        
        [HttpPost("deposit")]
        public async Task Deposit([FromQuery] DepositBalanceRequest dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            await _service.Deposit(userId, dto.Amount, dto.Description);
        }

        [HttpPost("withdraw")]
        public async Task Withdraw([FromQuery] WithdrawBalanceRequest dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            await _service.Withdraw(userId, dto.Amount, dto.Description);
        }

        [HttpGet("statement")]
        public async Task<List<BankStatement>> GetStatement([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        { 
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            var statement = await _service.GetStatementByDateRange(userId, startDate, endDate);
            return statement;
        }

        [HttpGet("balance")]
        public async Task<decimal?> GetBalance()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            var balance = await _service.GetBalance(userId);
            
            return balance;
        }

        [HttpPost("transfer/{phoneNumber}")]
        public async Task Transfer([FromRoute] string phoneNumber,[FromQuery] decimal amount)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();

            await _service.Transfer(userId, phoneNumber, amount);
        }
        
        [HttpDelete]
        public async Task DeleteUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            await _service.DeleteUser(userId);
        }

        [HttpPatch]
        public async Task UpdateUser(UpdateUserRequest userRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            await _service.UpdateUser(userId, userRequest);
        }
        
        [HttpPatch("reset")]
        public async Task ResetPassword([FromQuery] string email, [FromQuery] string password,
            [FromQuery] string newPassword, [FromQuery] string confirmPassword)
        {
            await _service.ResetPassword(email, password, newPassword, confirmPassword);
        }

        [HttpGet("user-details")]
        public async Task<UserDetailsResponse> GetUserDetails()
        { 
            var userId = User.FindFirst("User Id")?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            var user = await _service.GetUserDetails(userId);

            return user;
        }

        [HttpGet("transfer-details/{name}")]

        public async Task<List<BankStatement>>GetTransferDetail([FromRoute] string name)
        {
            var userId = User.FindFirst("User Id")?.Value;
            var user = await _service.TransferDetails(userId, name);
            return user;
        }

        [HttpGet("contact-details")]
        public async Task<ContactDetailsResponse> GetContactDetails()
        {
            var userId = User.FindFirst("User Id")?.Value;
            var response = await _service.GetContactDetails(userId);
            return response;
        }

        [HttpGet("card-details")]
        public async Task<CardDetailsResponse> ShowCardDetails(string password, string cardID)
        {
            var userId = User.FindFirst("User Id")?.Value;
            var card = _service.ShowCardDetails(userId, password, cardID);
            return await card;
        }

        [HttpPost("add-cards")]
        public async Task<AddCardsResponse> AddCards()
        {
            var userId = User.FindFirst("User Id")?.Value;
            return await _service.AddCard(userId);
        }

        [HttpPost("delete-cards")]

        public async Task DeleteCards([FromQuery] string cardId)
        {
            var userId = User.FindFirst("User Id")?.Value; 
            await _service.DeleteCard(cardId, userId);
        }

        [HttpPatch("blocked-cards/{cardId}")]
        public async Task BlockedCards([FromRoute] string cardId)
        {
            var userId = User.FindFirst("User Id")?.Value;
            await _service.BlockCard(cardId, userId);
        }

        [HttpPatch("activate-cards/{cardId}")]
        public async Task ActivateCard([FromRoute] string cardId)
        {
            var userId = User.FindFirst("User Id")?.Value;
            await _service.ActivateCard(cardId, userId);
        }
}