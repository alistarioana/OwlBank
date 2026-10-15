using Microsoft.AspNetCore.Mvc;
using OwlBank.Auth.Contracts.DTOs;
using OwlBank.Auth.Contracts.Interfaces;
using OwlBank.Extensions;
using OwlBank.Models;
using System.Security.Cryptography;

namespace OwlBank.Auth.Controllers;

[ApiController]
[Route("api")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _service;
        
    public LoginController(ILoginService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task AddUser([FromQuery] CreateUserRequest userRequest)
    {
        await _service.Register(userRequest);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(
    [FromBody] LoginRequest userRequest)
    {
        var token = await _service.Login(userRequest);


        return Ok(token);
    }

   

    [HttpPatch("resetpassword")]
    public async Task ResetPassword([FromQuery] string email, [FromQuery] string password,
        [FromQuery] string newPassword, [FromQuery] string confirmPassword)
    {
        await _service.ResetPassword(email, password, newPassword, confirmPassword);
    }
}
