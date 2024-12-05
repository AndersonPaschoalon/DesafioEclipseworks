namespace EclipseTaskManager.Unit.ControllersTests;

using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EclipseTaskManager.Controllers;
using EclipseTaskManager.Context;
using EclipseTaskManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EclipseTaskManager.Repository;
using EclipseTaskManager.Logging;
using Microsoft.Extensions.Logging;
using EclipseTaskManager.DTOs;


[TestFixture]
public class UsersControllerTestsGet
{
    private UsersController _controller;
    private static int idCount = 0;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<EclipseTaskManagerContext>()
            .UseInMemoryDatabase(databaseName: $"TestDatabase_{idCount}")
            .Options;

        var context = new EclipseTaskManagerContext(options);

        context.Users.AddRange(new User
        {
            Name = "Test User 1",
        }, new User
        {
            Name = "Test User 2",
        });

        UserRepository userRepository = new UserRepository(context);
        ProjectRepository projectRepository = new ProjectRepository(context);
        var mockLogger = new Mock<ILogger<UsersController>>();


        context.SaveChanges();
        _controller = new UsersController(userRepository, projectRepository, mockLogger.Object);
    }

    [Test]
    public void GetAll_ShouldReturnOk_WhenUsersExist()
    {

        // Act
        var result = _controller.GetAll();

        // Assert
        var actionResult = result as ActionResult<IEnumerable<UserDTO>>;
        var okResult = actionResult.Result as OkObjectResult;
        Assert.AreEqual(200, okResult?.StatusCode);
        var users = okResult?.Value as List<UserDTO>;

    }


    [Test]
    public void Get_ShouldReturnOk_WhenUserExists()
    {
        // Act
        var result = _controller.Get(1);

        // Assert
        var actionResult = result as ActionResult<UserDTO>;
        var okResult = actionResult.Result as OkObjectResult;
        Assert.AreEqual(200, okResult?.StatusCode);
        var user = okResult?.Value as UserDTO;
        Assert.AreEqual(1, user?.UserId);
    }


    [Test]
    public void Get_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Act
        var result = _controller.Get(999);

        // Assert
        var actionResult = result as ActionResult<UserDTO>;
        var notFoundResult = actionResult.Result as NotFoundObjectResult;
        Assert.AreEqual(404, notFoundResult?.StatusCode);
    }

    [Test]
    public void Post_ShouldReturnCreated_WhenUserIsValid()
    {
        // Arrange
        var userDto = new UserDTO
        {
            Name = "New User"
        };

        // Act
        var result = _controller.Post(userDto);

        // Assert
        var actionResult = result as ActionResult<UserDTO>;
        var createdResult = actionResult.Result as CreatedAtRouteResult;
        Assert.AreEqual(201, createdResult?.StatusCode);
    }

    [Test]
    public void Post_ShouldReturnBadRequest_WhenUserDtoIsNull()
    {
        // Act
        var result = _controller.Post(null);

        // Assert
        var actionResult = result as ActionResult<UserDTO>;
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        Assert.AreEqual(400, badRequestResult?.StatusCode);
    }




}
