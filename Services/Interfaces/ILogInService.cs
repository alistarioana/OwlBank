using Microsoft.AspNetCore.Identity.Data;
using OwlBank.DTOs.UserDTO;

namespace OwlBank.Services;

public interface ILogInService
{
    public Task<string> Login(LoginRequest userRequest);
    public Task Register(CreateUserRequest userRequest);
}