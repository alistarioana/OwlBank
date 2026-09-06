using System.Reflection;
using OwlBank.Repository;
using OwlBank.Services;

namespace OwlBank;

public static class ServiceExtensions
{
   public static IServiceCollection AddDependencies(this IServiceCollection services)
   {
        var assembly = Assembly.GetExecutingAssembly();

        var typesWithAttribute = assembly.GetTypes()
            .Select(t => new { Type = t, Attr = t.GetCustomAttribute<DependencyAttribute>() })
            .Where(x => x.Attr is not null);

        foreach (var x in typesWithAttribute)
        {
            services.Add(new ServiceDescriptor(x.Attr!.ServiceType, x.Type, x.Attr.Lifetime));
        }

      return services;
   }
}
