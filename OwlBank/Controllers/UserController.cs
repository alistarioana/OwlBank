using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OwlBank.DTOs.UserDTO;
using OwlBank.Extensions;
using OwlBank.Models;
using OwlBank.Services;

namespace OwlBank.Controllers;

[EnableRateLimiting("fixedWindow")]
[ApiController]
[Route("users")]
[Authorize(Roles = nameof(Role.User))]
public class UserController : ControllerBase
{
        private readonly IUserService _service;

        private Guid UserId => User.GetUserId();

        public UserController(IUserService service)
        {
            _service = service;
        }
        
        [HttpPost("deposit")]
        public async Task Deposit([FromQuery] DepositBalanceRequest dto) => 
            await _service.Deposit(UserId, dto.Amount, dto.Description);

        [HttpPost("withdraw")]
        public async Task Withdraw([FromQuery] WithdrawBalanceRequest dto)
        {
            await _service.Withdraw(UserId, dto.Amount, dto.Description);
        }

        [HttpGet("statement")]
        public async Task<List<BankStatement>> GetStatement([FromQuery] DateTime startDate, [FromQuery] DateTime endDate) => 
             await _service.GetStatementByDateRange(UserId, startDate, endDate);

        [HttpGet("balance")]
        public async Task<decimal> GetBalance() => 
           await _service.GetBalance(UserId);
            

        [HttpPost("transfer/{phoneNumber}")]
        public async Task Transfer([FromRoute] string phoneNumber,[FromQuery] decimal amount) => 
            await _service.Transfer(UserId, phoneNumber, amount);
        
        [HttpDelete]
        public async Task DeleteUser() => 
            await _service.DeleteUser(UserId);

        [HttpPatch]
        public async Task UpdateUser(UpdateUserRequest userRequest) => 
            await _service.UpdateUser(UserId, userRequest);
        
        [HttpPatch("reset")]
        public async Task ResetPassword(
            [FromQuery] string email,
            [FromQuery] string password,
            [FromQuery] string newPassword,
            [FromQuery] string confirmPassword)
            => await _service.ResetPassword(email, password, newPassword, confirmPassword);

        [HttpGet("user-details")]
        public async Task<UserDetailsResponse> GetUserDetails() => 
            await _service.GetUserDetails(UserId);

        [HttpGet("transfer-details/{name}")]
        public async Task<List<BankStatement>>GetTransferDetail([FromRoute] string name) => 
            await _service.TransferDetails(UserId, name);

        [HttpGet("contact-details")]
        public async Task<ContactDetailsResponse> GetContactDetails() =>
            await _service.GetContactDetails(UserId);

        [HttpGet("card-details")]
        public async Task<CardDetailsResponse> ShowCardDetails(string password, string cardID) => 
         await _service.ShowCardDetails(UserId, password, cardID);

        [HttpGet("card-details-back")]
        public async Task<CardDetailsResponse> ShowCardDetailsBack(string password,string cardID) => 
            await _service.ShowCardDetails(UserId, password, cardID);

        [Authorize]
        [HttpPost("add-cards")]
        public async Task<AddCardsResponse> AddCards() => 
            await _service.AddCard(UserId);

        [HttpPost("delete-cards")]
        public async Task DeleteCards([FromQuery] string cardId) =>
            await _service.DeleteCard(cardId, UserId);

        [HttpPatch("blocked-cards/{cardId}")]
        public async Task BlockedCards([FromRoute] string cardId) =>
            await _service.BlockCard(cardId, UserId);

        [HttpPatch("activate-cards/{cardId}")]
        public async Task ActivateCard([FromRoute] string cardId) =>
             await _service.ActivateCard(cardId, UserId);

        [HttpGet("transactions")]
        public async Task<List<Transaction>> GetTransactions() => 
             await _service.GetTransactionsAsync(UserId);

        [HttpGet("get-all-cards")]
        public async Task<List<Card>> GetAllCards() => 
            await _service.GetAllCardsAsync(UserId);

    [AllowAnonymous]
    [HttpPost("regenerate-token")]
    public async Task<ActionResult<string>> RegenerateToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var token = await _service.RegenerateTokens(refreshToken);

        return Ok(token);   
    }
}