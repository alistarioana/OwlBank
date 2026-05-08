using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OwlBank.DTOs.UserDTO;
namespace OwlBank.Controllers;
using OwlBank.Models;
using OwlBank.Services;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        
        [HttpPost("register")]
        public async Task AddUser(CreateUserRequest userRequest)
        { 
            await _service.AddUser(userRequest);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest userRequest)
        {
            var token = await _service.Login(userRequest);
            return Ok(token);
        }
        
        [Authorize]
        [HttpPost("{id}/deposit")]
        public async Task<IActionResult> Deposit(Guid id, DepositBalanceRequest dto)
        {
            await _service.Deposit(id, dto.Amount, dto.Description);
            return Ok();
        }

        [Authorize]
        [HttpPost("{id}/withdraw")]
        public async Task<IActionResult> Withdraw(Guid id, WithdrawBalanceRequest dto)
        {
            await _service.Withdraw(id, dto.Amount, dto.Description);
            return Ok();
        }
        
        [HttpDelete]
        public async Task DeleteUser(Guid id)
        {
            await _service.DeleteUser(id);
        }

        [HttpPatch]
        public async Task UpdateUser(Guid id, UpdateUserRequest userRequest)
        {
            await _service.UpdateUser(id, userRequest);
        }
}