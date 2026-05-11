using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OwlBank.DTOs.UserDTO;
using OwlBank.Services;

namespace OwlBank.Controllers;

[ApiController]
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
    public async Task<IActionResult> Login([FromQuery] LoginRequest userRequest)
    {
        var token = await _service.Login(userRequest);
        return Ok(token);
    }
}