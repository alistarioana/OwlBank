using Microsoft.IdentityModel.Tokens;
using OwlBank.Models;
using OwlBank.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OwlBank.Services.Implementations
{
    [Dependency(typeof(IRefreshTokenCookieService))]
    public class RefreshTokenCookieService : IRefreshTokenCookieService
    {
        private const string CookieName = "refreshToken";
        private readonly IHttpContextAccessor _http;
        private readonly IConfiguration _config;

        public RefreshTokenCookieService(IHttpContextAccessor http, IConfiguration config) 
            {
                _http = http;
                _config = config;
            }


        public void SetRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresDate = DateTime.UtcNow.AddDays(7)
            };

            _http.HttpContext!.Response.Cookies.Append(CookieName, refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.ExpiresDate
            });

            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenExpireDate = refreshToken.ExpiresDate;


        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("User Id", user.ID.ToString()),
                new Claim("Email", user.Email)

            };

            foreach (var role in user.UserRoles)
            {
                claims.Add(new Claim("Roles", role.ToString()));

            }
            var secret = _config["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}