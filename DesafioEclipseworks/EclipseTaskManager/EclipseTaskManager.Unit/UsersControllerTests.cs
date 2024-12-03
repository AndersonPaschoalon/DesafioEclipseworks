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

namespace EclipseTaskManager.Tests
{
    [TestFixture]
    public class UsersControllerTests
    {
        private UsersController _controller;


        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<EclipseTaskManagerContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new EclipseTaskManagerContext(options);

            context.Users.AddRange(new User
            {
                UserId = 1,
                Name = "Test User 1",
            }, new User
            {
                UserId = 2,
                Name = "Test User 2",
            });

            context.SaveChanges();
            _controller = new UsersController(context);
        }

        [Test]
        public void GetAll_ShouldReturnOk_WhenUsersExist()
        {
            // Act
            var result =  _controller.GetAll();

            // Assert
            var actionResult = result as ActionResult<IEnumerable<User>>;
            var okResult = actionResult.Result as OkObjectResult;
            Assert.AreEqual(200, okResult?.StatusCode);
            var users = okResult?.Value as List<User>;
            Assert.AreEqual(2, users?.Count);
        }


    }
}
