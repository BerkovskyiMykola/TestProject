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
        public readonly Mock<IJwtService> _jwtService = new Mock<IJwtService>();
        public readonly Mock<IPasswordHasher<User>> _passwordHasher = new Mock<IPasswordHasher<User>>();
        public readonly ApplicationContext _context;
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
            var sut = new AuthController(_context, _jwtService.Object, _passwordHasher.Object);

            //Act
            var response = await sut.Register(user);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async void Should_not_register_two_same_users()
        {
            //Arrange
            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var sut = new AuthController(_context, _jwtService.Object, _passwordHasher.Object);

            //Act
            await sut.Register(user);
            var response = await sut.Register(user);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal("User with such Email exists", badRequestObjectResult.Value);
        }

        [Fact]
        public async void Should_authorize_a_registered_user()
        {
            //Arrange
            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test1" };
            var sut = new AuthController(_context, _jwtService.Object, _passwordHasher.Object);

            //Act
            await sut.Register(user);
            var response = await sut.Authenticate(auth);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async void Should_not_authorize_an_unresgistered_user()
        {
            //Arrange
            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test1" };
            var sut = new AuthController(_context, _jwtService.Object, _passwordHasher.Object);

            //Act
            var response = await sut.Authenticate(auth);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal("Email or password is incorrect", badRequestObjectResult.Value);
        }

        [Fact]
        public async void Should_not_authorize_a_user_with_wrong_password()
        {
            //Arrange
            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test2" };
            var sut = new AuthController(_context, _jwtService.Object, _passwordHasher.Object);

            //Act
            await sut.Register(user);
            var response = await sut.Authenticate(auth);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal("Email or password is incorrect", badRequestObjectResult.Value);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}