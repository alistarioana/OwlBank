using Microsoft.AspNetCore.Authorization;

namespace OwlBank.Controllers;
using Microsoft.AspNetCore.Mvc;
using OwlBank.DTOs.UserDTO;
using OwlBank.Models;
using OwlBank.Services;

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
    public async Task UpdateRole(Guid id, List<string> roles)
    {
        await _service.UpdateUserRole(id, roles);
    }

    [HttpPatch("reset-password/{id}")]
    public async Task ResetPassword([FromRoute] Guid id)
    {
        await _service.ResetPassword(id);
    }

    [HttpDelete("delete-user/{userId}")]
    public async Task DeleteUser([FromRoute] Guid userId)
    {
        await _service.DeleteUser(userId);
    }
}