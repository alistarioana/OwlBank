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

    [HttpPost("roles")]
    public async Task UpdateRole(string id, List<string> roles)
    {
        await _service.UpdateUserRole(id, roles);
    }

    [HttpPatch("reset-password/{id}")]
    public async Task UpdatePassword([FromRoute] string id)
    {
        await _service.UpdatePassword(id);
    }

    [HttpDelete("delete-user/{userId}")]
    public async Task DeleteUser([FromRoute] string userId)
    {
        await _service.DeleteUser(userId);
    }
}