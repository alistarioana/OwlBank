using Microsoft.AspNetCore.Mvc;
using Moq;
using OwlBank.Controllers;
using OwlBank.Auth.Contracts.DTOs;
using OwlBank.Auth.Contracts.Interfaces;

namespace TestProject1.ControllerTests;

public class LoginControllerTests
{
    
    //[Fact]
    //public async Task AddUser_ShouldCreateUser()
    //{
    //    Mock<ILoginService> service = new Mock<ILoginService>();
    //    CreateUserRequest userRequest = new CreateUserRequest();
    //    service.Setup(x => x.Register(userRequest));
        
    //    LoginController controller = new LoginController(service.Object);
    //    controller.AddUser(userRequest);
        
    //    service.Verify(x => x.Register(userRequest), Times.Once);
    //}
}