using System.Net.Http.Json;
using OwlBank.Auth.Contracts.DTOs;
using OwlBank.Auth.Contracts.Interfaces;

namespace OwlBank.Services.Implementations;

public class AuthHttpLoginService : OwlBank.Auth.Contracts.Interfaces.ILoginService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthHttpLoginService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> Login(LoginRequest userRequest)
    {
        var client = _httpClientFactory.CreateClient("Auth");
        var resp = await client.PostAsJsonAsync("api/login", userRequest);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<string>() ?? string.Empty;
    }

    public async Task Register(CreateUserRequest userRequest)
    {
        var client = _httpClientFactory.CreateClient("Auth");
        var resp = await client.PostAsJsonAsync("api/register", userRequest);
        resp.EnsureSuccessStatusCode();
    }

    public async Task ResetPassword(string email, string password, string newpassword, string confirmPassword)
    {
        var client = _httpClientFactory.CreateClient("Auth");
        var url = $"api/resetpassword?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}&newPassword={Uri.EscapeDataString(newpassword)}&confirmPassword={Uri.EscapeDataString(confirmPassword)}";
        var resp = await client.PatchAsync(url, null);
        resp.EnsureSuccessStatusCode();
    }
}
