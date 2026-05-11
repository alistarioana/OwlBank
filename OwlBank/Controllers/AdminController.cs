using Microsoft.AspNetCore.Authorization;
using OwlBank.DTOs.UserDTO;
using OwlBank.Models;
using OwlBank.Services;
namespace OwlBank.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("users")]
[Authorize(Roles = $"{nameof(Role.Manager)}, {nameof(Role.Admin)}")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _service;

    public AdminController(IAdminService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<List<UserResponse>> GetUsers()
    {
        var result = await _service.GetUsers();
        return result;
    }
}