using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using OwlBank.Auth.Contracts.Interfaces;
using OwlBank.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the OwlBank database context (required by UserRepository)
builder.AddNpgsqlDbContext<OwlBankDBContext>("OwlBankDb");

// configure JWT token validation (uses same configuration key as main app if set)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "dev-key-placeholder")),
            RoleClaimType = "Roles"
        };
    });

builder.Services.AddAuthorization();

// Register application services from the referenced OwlBank project
builder.Services.AddScoped<ILoginService, OwlBank.Auth.Services.LoginService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OwlBank Auth v1");
    c.EnablePersistAuthorization();
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
