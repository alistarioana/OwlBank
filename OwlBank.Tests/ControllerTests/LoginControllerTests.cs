using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OwlBank.Controllers;
using OwlBank.DTOs.UserDTO;
using OwlBank.Services;

namespace TestProject1.ControllerTests;

public class LoginControllerTests
{
    [Fact]
    public async Task Login_ShouldReturnToken()
    {
        //Arrange
        Mock<ILoginService> service = new Mock<ILoginService>();
        LoginRequest login = new LoginRequest()
        {
            Email = "email",
            Password = "password"
        };
        string token = "digbndfbgsiiudhnvfdsldobfjpdgbjohitigv";
        service.Setup(x => x.Login(login)).ReturnsAsync(token);
        
        //Act
        LoginController controller = new LoginController(service.Object);
        IActionResult result = await controller.Login(login);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(token, okResult.Value);
    }
    
    [Fact]
    public async Task AddUser_ShouldCreateUser()
    {
        Mock<ILoginService> service = new Mock<ILoginService>();
        CreateUserRequest userRequest = new CreateUserRequest();
        service.Setup(x => x.Register(userRequest));
        
        LoginController controller = new LoginController(service.Object);
        controller.AddUser(userRequest);
        
        service.Verify(x => x.Register(userRequest), Times.Once);
    }
}