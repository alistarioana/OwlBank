using System.Threading.Tasks;
using OwlBank.Auth.Contracts.DTOs;

namespace OwlBank.Auth.Contracts.Interfaces;

public interface ILoginService
{
    public Task<string> Login(LoginRequest userRequest);
    public Task Register(CreateUserRequest userRequest);
    public Task ResetPassword(string email, string password, string newpassword, string confirmPassword);
}
