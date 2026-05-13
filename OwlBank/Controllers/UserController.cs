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
        public async Task<IActionResult> Deposit( [FromQuery]DepositBalanceRequest dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            await _service.Deposit(userId, dto.Amount, dto.Description);
            return Ok();
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromQuery]WithdrawBalanceRequest dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            await _service.Withdraw(userId, dto.Amount, dto.Description);
            return Ok();
        }

        [HttpGet("statement")]
        public async Task<IActionResult> GetStatement([FromQuery]DateTime startDate, [FromQuery]DateTime endDate)
        { 
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            var statement = await _service.GetStatementByDateRange(userId, startDate, endDate);
            return Ok(statement);
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            var balance = await _service.GetBalance(userId);
            
            return Ok(balance);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(string phoneNumber, decimal amount)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            await _service.Transfer(userId, phoneNumber, amount);
            return Ok();
        }
        
        [HttpDelete]
        public async Task DeleteUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            await _service.DeleteUser(userId);
        }

        [HttpPatch]
        public async Task UpdateUser( UpdateUserRequest userRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) throw new UnauthorizedAccessException();
            
            await _service.UpdateUser(userId, userRequest);
        }
}