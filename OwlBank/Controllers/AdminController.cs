using Microsoft.AspNetCore.Authorization;

namespace OwlBank.Controllers;
using OwlBank.Models;
using OwlBank.Services;
using Microsoft.AspNetCore.Mvc;
using OwlBank.DTOs.UserDTO;

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
        return await _service.GetUsers();
    }
}