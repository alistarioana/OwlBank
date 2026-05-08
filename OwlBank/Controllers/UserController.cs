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
[Authorize(Roles = "User")]
public class UserController : ControllerBase
{
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        
        [HttpPost("{id}/deposit")]
        public async Task<IActionResult> Deposit(Guid id, DepositBalanceRequest dto)
        {
            await _service.Deposit(id, dto.Amount, dto.Description);
            return Ok();
        }

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