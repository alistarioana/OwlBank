using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OwlBank.Repository;

namespace TestProject1.ControllerTests;

public class TestFactory: WebApplicationFactory<Program>
{
    public Mock<IUserRepository> UserRepoMock { get; } = new();
    public Mock<IBankStatementRepository> BankRepoMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var userRepoDescriptor = services
                .SingleOrDefault(x => x.ServiceType == typeof(IUserRepository));

            if (userRepoDescriptor != null)
                services.Remove(userRepoDescriptor);

            var bankRepoDescriptor = services
                .SingleOrDefault(x => x.ServiceType == typeof(IBankStatementRepository));

            if (bankRepoDescriptor != null)
                services.Remove(bankRepoDescriptor);

            services.AddSingleton(UserRepoMock.Object);
            services.AddSingleton(BankRepoMock.Object);
        });
    }
}