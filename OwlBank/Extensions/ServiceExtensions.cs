using System.Reflection;
using OwlBank.Repository;
using OwlBank.Services;

namespace OwlBank;

public static class ServiceExtensions
{
   public static IServiceCollection AddDependencies(this IServiceCollection services)
   {
      services.AddScoped<IBankStatementRepository, BankStatementRepository>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IAdminRepository, AdminRepository>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IAdminService, AdminService>();
      services.AddScoped<ILoginService, LoginService>();
      services.AddScoped<ICardRepository, CardRepository>();
      
      return services;
   }
}

public class DependencyAttribute(Type type, ServiceLifetime lifetime = ServiceLifetime.Scoped) : Attribute
{
   
}