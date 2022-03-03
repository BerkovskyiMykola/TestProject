using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestProject.Controllers;
using TestProject.Models;
using TestProject.Models.Request;
using TestProject.Services.Authorization;
using TestProject.Services.Authorization.Models;
using Xunit;

namespace TestProject.Tests
{
    public class AuthControllerTests
    {
        public readonly Mock<IJwtService> _jwtService = new Mock<IJwtService>();
        public readonly Mock<IPasswordHasher<User>> _passwordHasher = new Mock<IPasswordHasher<User>>();
        public AuthControllerTests()
        {
            _jwtService
                .Setup(x => x.GetToken(It.IsAny<JwtUser>())).Returns(It.IsAny<string>());

            _passwordHasher
                .Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns((User user, string pass) => pass);
            _passwordHasher
                .Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((User user, string aPass, string bPass) =>
                aPass == bPass ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed);
        }

        [Fact]
        public async void Register_Should_Register_User()
        {
            //Arrage
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "AuthControllerDB_1")
                .Options;

            using var dbContext = new ApplicationContext(options);

            var role = new Role { Id = Guid.NewGuid(), Name = "User" };

            dbContext.Add(role);
            dbContext.SaveChanges();

            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var controller = new AuthController(dbContext, _jwtService.Object, _passwordHasher.Object);

            //Act
            var response = await controller.Register(user);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async void Register_Shouldnt_Register_2_Same_Users()
        {
            //Arrage
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "AuthControllerDB_2")
                .Options;

            using var dbContext = new ApplicationContext(options);

            var role = new Role { Id = Guid.NewGuid(), Name = "User" };

            dbContext.Add(role);
            dbContext.SaveChanges();

            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };

            var controller = new AuthController(dbContext, _jwtService.Object, _passwordHasher.Object);

            //Act
            await controller.Register(user);
            var response = await controller.Register(user);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal("User with such Email exists", badRequestObjectResult.Value);
        }

        [Fact]
        public async void Authenticate_Should_Authorize_Resgistered_User()
        {
            //Arrage
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "AuthControllerDB_3")
                .Options;

            using var dbContext = new ApplicationContext(options);

            var role = new Role { Id = Guid.NewGuid(), Name = "User" };

            dbContext.Add(role);
            dbContext.SaveChanges();

            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test1" };

            var controller = new AuthController(dbContext, _jwtService.Object, _passwordHasher.Object);

            //Act
            await controller.Register(user);
            var response = await controller.Authenticate(auth);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async void Authenticate_Shouldnt_Authorize_Unresgistered_User()
        {
            //Arrage
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "AuthControllerDB_4")
                .Options;

            using var dbContext = new ApplicationContext(options);

            var role = new Role { Id = Guid.NewGuid(), Name = "User" };

            dbContext.Add(role);
            dbContext.SaveChanges();

            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test1" };

            var controller = new AuthController(dbContext, _jwtService.Object, _passwordHasher.Object);

            //Act
            var response = await controller.Authenticate(auth);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal("Email or password is incorrect", badRequestObjectResult.Value);
        }

        [Fact]
        public async void Authenticate_Shouldnt_Authorize_User_With_Wrong_Password()
        {
            //Arrage
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "AuthControllerDB_5")
                .Options;

            using var dbContext = new ApplicationContext(options);

            var role = new Role { Id = Guid.NewGuid(), Name = "User" };

            dbContext.Add(role);
            dbContext.SaveChanges();

            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test2" };

            var controller = new AuthController(dbContext, _jwtService.Object, _passwordHasher.Object);

            //Act
            await controller.Register(user);
            var response = await controller.Authenticate(auth);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal("Email or password is incorrect", badRequestObjectResult.Value);
        }
    }
}