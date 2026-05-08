namespace OwlBank.Controllers;
using OwlBank.Models;
using OwlBank.Services;
using Microsoft.AspNetCore.Mvc;
using OwlBank.DTOs.UserDTO;


[ApiController]
[Route("users")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _service;

    public AdminController(IAdminService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<List<UserResponse>> GetTasks()
    {
        return await _service.GetUsers();
    }
}