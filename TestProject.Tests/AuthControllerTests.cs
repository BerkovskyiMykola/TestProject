using FluentAssertions;
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
    public class AuthControllerTests : IDisposable
    {
        public readonly Mock<IJwtService> _stubJwtService = new Mock<IJwtService>();
        public readonly Mock<IPasswordHasher<User>> _stubPasswordHasher = new Mock<IPasswordHasher<User>>();
        public readonly ApplicationContext _context;
        public AuthControllerTests()
        {
            _stubJwtService
                .Setup(x => x.GetToken(It.IsAny<JwtUser>())).Returns(It.IsAny<string>());

            _stubPasswordHasher
                .Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns((User user, string pass) => pass);
            _stubPasswordHasher
                .Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((User user, string aPass, string bPass) =>
                    aPass == bPass ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed);

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationContext(options);
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async void Should_register_an_user()
        {
            //Arrange
            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var sut = new AuthController(_context, _stubJwtService.Object, _stubPasswordHasher.Object);

            //Act
            var result = await sut.Register(user);
            var userIsAdded = _context.Users.Any(x => x.Email == user.Email && x.Password == user.Password);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            userIsAdded.Should().BeTrue();
        }

        [Fact]
        public async void Should_not_register_many_same_users()
        {
            //Arrange
            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var sut = new AuthController(_context, _stubJwtService.Object, _stubPasswordHasher.Object);

            await sut.Register(user);

            //Act
            var result = await sut.Register(user);
            var amountRegisteredUsers = _context.Users.Count(x => x.Email == user.Email && x.Password == user.Password);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            amountRegisteredUsers.Should().Be(1);
        }

        [Fact]
        public async void Should_authorize_a_registered_user()
        {
            //Arrange
            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test1" };
            var sut = new AuthController(_context, _stubJwtService.Object, _stubPasswordHasher.Object);

            await sut.Register(user);

            //Act
            var response = await sut.Authenticate(auth);

            //Assert
            response.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Should_not_authorize_an_unresgistered_user()
        {
            //Arrange
            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test1" };
            var sut = new AuthController(_context, _stubJwtService.Object, _stubPasswordHasher.Object);

            //Act
            var response = await sut.Authenticate(auth);

            //Assert
            response.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void Should_not_authorize_a_user_with_wrong_password()
        {
            //Arrange
            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test2" };
            var sut = new AuthController(_context, _stubJwtService.Object, _stubPasswordHasher.Object);

            await sut.Register(user);

            //Act
            var response = await sut.Authenticate(auth);

            //Assert
            response.Should().BeOfType<BadRequestObjectResult>();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}