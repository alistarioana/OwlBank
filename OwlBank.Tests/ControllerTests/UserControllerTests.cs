using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using OwlBank.Controllers;
using OwlBank.DTOs.UserDTO;
using OwlBank.Models;
using OwlBank.Repository;
using OwlBank.Services;

namespace TestProject1.ControllerTests;

public class UserControllerTests:IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserControllerTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Withdraw_ShouldWithdrawXAmount()
    {
        //Arrange
        Mock<IUserService> userService = new Mock<IUserService>();
        userService.Setup(x => x.Withdraw(Guid.NewGuid().ToString(), 20, "ok"));
        //Act
        UserController controller = new UserController(userService.Object);
        WithdrawBalanceRequest dto = new WithdrawBalanceRequest();
        dto.Amount = 20;
        IActionResult result = await controller.Withdraw(dto);
        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Successfully withdraw: 20", okResult.Value);
    }
    
    [Fact]
    public async Task DeleteUser_ShouldDeleteUser()
    {
        //Arrange
        Mock<IUserService> userService = new Mock<IUserService>();
        userService.Setup(x => x.DeleteUser(It.IsAny<string>()));
        //Act
        UserController controller = new UserController(userService.Object);
        await controller.DeleteUser();
        //Assert
        userService.Verify(x => x.DeleteUser(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateUser_ShouldUpdateUser()
    {
        //Arrange
        Mock<IUserService> userService = new Mock<IUserService>();
        
        //Act
        UserController controller = new UserController(userService.Object);
        await controller.DeleteUser();
        //Assert
        userService.Verify(x => x.DeleteUser(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Deposit_ShouldThrowUserNotFoundException_When_UserTheseNotExist()
    {
        Mock<IUserRepository > userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.GetUserById(It.IsAny<string>())).ReturnsAsync(null as User);
        var id = Guid.NewGuid();
        var request = new DepositBalanceRequest
        {
            Amount = 2,
            Description = "Test"
        };
              
        //Act
        var response = await _client.PostAsJsonAsync(
            $"/users/deposit",
            request);
        
        //Assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Contains("User not found", content);
    }

  [ Fact]
   public async Task Deposit_ShouldThrowInvalidAmountException_When_AmountIsLessThanZero()
   {
       // Arrange
       var factory = new TestFactory();
   
       factory.UserRepoMock
           .Setup(x => x.GetUserById(It.IsAny<string>()))
           .ReturnsAsync(new User()); 
   
       var client = factory.CreateClient();
   
       var id = Guid.NewGuid();
   
    var request = new DepositBalanceRequest
       {
           Amount = -100,
           Description = "Test"
       };

       // Act
       var response = await client.PostAsJsonAsync(
           $"/users/deposit",
           request);
       
       //Assert                                                                   
       var content = await response.Content.ReadAsStringAsync();                  
       Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);     
       Assert.Contains("Invalid amount", content);                                
   }
   
   [Fact]
   public async Task Deposit_ShouldDepositXAmount()
   {
       // Arrange
       var factory = new TestFactory();

       var user = new User
       {
           ID = Guid.NewGuid(),
           Balance = 50
       };

       factory.UserRepoMock
           .Setup(x => x.GetUserById(It.IsAny<string>()))
           .ReturnsAsync(user);

       var client = factory.CreateClient();

       var request = new DepositBalanceRequest
       {
           Amount = 100,
           Description = "Test"
       };

       // Act
       var response = await client.PostAsJsonAsync(
           $"/users/deposit",
           request);

       // Assert
       Assert.Equal(HttpStatusCode.OK, response.StatusCode);

       // Balance should increase
       Assert.Equal(150, user.Balance);

       // SaveChanges should be called
       factory.UserRepoMock.Verify(
           x => x.SaveChanges(),
           Times.Once);

       // DepositAction should be called
       factory.BankRepoMock.Verify(
           x => x.DepositAction(It.IsAny<BankStatement>()),
           Times.Once);
   }                                                                                     
}