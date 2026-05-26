using Microsoft.AspNetCore.Identity.Data;
using OwlBank.DTOs.UserDTO;

namespace OwlBank.Services;

public interface ILoginService
{
    public Task<string> Login(LoginRequest userRequest);
    public Task Register(CreateUserRequest userRequest);
    public Task ResetPassword(string email, string password, string newpassword, string confirmPassword);
}