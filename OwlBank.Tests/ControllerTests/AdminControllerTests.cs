using System.Numerics;
using Moq;
using OwlBank.Controllers;
using OwlBank.DTOs.UserDTO;
using OwlBank.Services;

namespace TestProject1.ControllerTests;

public class AdminControllerTests
{
    [Fact]
    public async Task GetUsers_ShouldReturnListOfUsers()
    {
        //Arrange
       Mock<IAdminService> service = new Mock<IAdminService>();
       List<UserResponse> userList = new List<UserResponse>();
       UserResponse userResponse = new UserResponse();
       userResponse.Email = "sgaby100@gmail.com";
       userList.Add(userResponse);
       
       service.Setup(x=> x.GetUsers()).ReturnsAsync(userList);
       
        //Act
        AdminController controller = new AdminController(service.Object);
        List<UserResponse> response = await controller.GetUsers();
        
        //Assert
        Assert.Single(response);
        Assert.Equal("sgaby100@gmail.com",  response[0].Email);
    }
}