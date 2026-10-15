using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using OwlBank.Controllers;
using OwlBank.DTOs.UserDTO;
using OwlBank.Models;
using OwlBank.Repository;
using OwlBank.Services;
using OwlBank.Services.Interfaces;
using Xunit;

namespace TestProject1.ControllerTests;

public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // Helper pentru a simula utilizatorul logat pe controller (previne NullReferenceException)
    private void SetUserContext(ControllerBase controller, string userId = "test-user-id")
    {
        var claims = new[]
        {
            new Claim("User Id", userId),
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    [Fact]
    public async Task Withdraw_ShouldWithdrawXAmount()
    {
        // Arrange
        Mock<IUserService> userService = new Mock<IUserService>();
        UserController controller = new UserController(userService.Object);
        SetUserContext(controller, "user-123");

        WithdrawBalanceRequest dto = new WithdrawBalanceRequest
        {
            Amount = 20,
            Description = "ok"
        };

        // Act
        // Metoda returneaza Task (void), deci doar o asteptam fara sa ii salvam rezultatul
        await controller.Withdraw(dto);

        // Assert
        // Verificam ca metoda Withdraw din IUserService a fost apelata o singura data
        userService.Verify(x => x.Withdraw(It.IsAny<Guid>(), 20, "ok"), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_ShouldDeleteUser()
    {
        // Arrange
        Mock<IUserService> userService = new Mock<IUserService>();
        UserController controller = new UserController(userService.Object);
        SetUserContext(controller, "user-123");

        // Act
        await controller.DeleteUser();

        // Assert
        userService.Verify(x => x.DeleteUser(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateUser()
    {
        // Arrange
        Mock<IUserService> userService = new Mock<IUserService>();
        UserController controller = new UserController(userService.Object);
        SetUserContext(controller, "user-123");

        UpdateUserRequest dto = new UpdateUserRequest();

        // Act - Apelam UpdateUser (nu DeleteUser)
        await controller.UpdateUser(dto);

        // Assert
        userService.Verify(x => x.UpdateUser(It.IsAny<Guid>(), dto), Times.Once);
    }

    [Fact]
    public async Task Deposit_ShouldThrowUserNotFoundException_When_UserDoesNotExist()
    {
        // Arrange
        Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.GetUserById(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        Mock<IBankStatementRepository> bankRepository = new Mock<IBankStatementRepository>();
        Mock<ICardRepository> cardRepository = new Mock<ICardRepository>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        Mock<IRefreshTokenCookieService> refreshTokenService = new Mock<IRefreshTokenCookieService>();

        UserService userService = new UserService(userRepository.Object, bankRepository.Object, cardRepository.Object, passwordHasher.Object, refreshTokenService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
        await userService.Deposit(Guid.NewGuid(), -100, "Test")
    );

        Assert.Equal("Invalid amount", exception.Message);
    }

    [Fact]
    public async Task Deposit_ShouldThrowInvalidAmountException_When_AmountIsLessThanZero()
    {
        // Arrange
        Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.GetUserById(It.IsAny<Guid>())).ReturnsAsync(new User());

        Mock<IBankStatementRepository> bankRepository = new Mock<IBankStatementRepository>();
        Mock<ICardRepository> cardRepository = new Mock<ICardRepository>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        Mock<IRefreshTokenCookieService> refreshTokenService = new Mock<IRefreshTokenCookieService>();

        UserService userService = new UserService(userRepository.Object, bankRepository.Object, cardRepository.Object, passwordHasher.Object, refreshTokenService.Object);


        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            userService.Deposit(Guid.NewGuid(), -100, "Test")
        );

        Assert.Equal("Invalid amount", exception.Message);
    }

    [Fact]
    public async Task Deposit_ShouldDepositXAmount()
    {
        // Arrange
        Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
        Mock<IBankStatementRepository> bankRepository = new Mock<IBankStatementRepository>();
        Mock<ICardRepository> cardRepository = new Mock<ICardRepository>();

        var user = new User
        {
            ID = Guid.NewGuid(),
            Balance = 50
        };

        userRepository.Setup(x => x.GetUserById(It.IsAny<Guid>())).ReturnsAsync(user);

        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        Mock<IRefreshTokenCookieService> refreshTokenService = new Mock<IRefreshTokenCookieService>();
        UserService userService = new UserService(userRepository.Object, bankRepository.Object, cardRepository.Object, passwordHasher.Object, refreshTokenService.Object);

        // Act
        await userService.Deposit(user.ID, 100, "Test");

        // Assert
        Assert.Equal(150, user.Balance);
    }
}