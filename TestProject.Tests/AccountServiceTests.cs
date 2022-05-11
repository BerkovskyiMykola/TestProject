using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestProject.BLL.MappingProfiles;
using TestProject.BLL.Services.Account;
using TestProject.BLL.Services.JWT;
using TestProject.BLL.Services.JWT.Models;
using TestProject.DAL.EF;
using TestProject.DAL.Entities;
using TestProject.DTO.Request;
using TestProject.DTO.Response;
using Xunit;

namespace TestProject.Tests
{
    public class AccountServiceTests : IDisposable
    {
        public readonly Mock<IJwtService> _stubJwtService = new Mock<IJwtService>();
        public readonly Mock<IPasswordHasher<User>> _stubPasswordHasher = new Mock<IPasswordHasher<User>>();
        public readonly Mock<IHttpContextAccessor> _stubHttpContextAccessor = new Mock<IHttpContextAccessor>();
        public readonly ApplicationContext _context;
        public readonly IMapper _mapper;
        public AccountServiceTests()
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

            var config = new MapperConfiguration(cgf => cgf.AddProfile<UserMappingProfile>());
            _mapper = new Mapper(config);

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
            var sut = new AccountService(
                _context, 
                _stubJwtService.Object, 
                _stubPasswordHasher.Object, 
                _stubHttpContextAccessor.Object, 
                _mapper
            );

            //Act
            var result = await sut.RegisterAsync(user);
            var userIsAdded = _context.Users.Any(x => x.Email == user.Email && x.Password == user.Password);

            //Assert
            result.Should().BeOfType<AuthorizeResponse>();
            userIsAdded.Should().BeTrue();
        }

        [Fact]
        public async void Should_not_register_many_same_users()
        {
            //Arrange
            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var sut = new AccountService(
                _context,
                _stubJwtService.Object,
                _stubPasswordHasher.Object,
                _stubHttpContextAccessor.Object,
                _mapper
            );

            await sut.RegisterAsync(user);

            //Act
            var test = sut.RegisterAsync(user);
            var amountRegisteredUsers = _context.Users.Count(x => x.Email == user.Email && x.Password == user.Password);

            //Assert
            amountRegisteredUsers.Should().Be(1);
        }

        [Fact]
        public async void Should_authenticate_a_registered_user()
        {
            //Arrange
            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test1" };
            var sut = new AccountService(
                _context,
                _stubJwtService.Object,
                _stubPasswordHasher.Object,
                _stubHttpContextAccessor.Object,
                _mapper
            );

            await sut.RegisterAsync(user);

            //Act
            var response = await sut.AuthenticateAsync(auth);

            //Assert
            response.Should().BeOfType<AuthorizeResponse>();
        }

        [Fact]
        public async void Should_not_authenticate_an_unresgistered_user()
        {
            //Arrange
            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test1" };
            var sut = new AccountService(
                _context,
                _stubJwtService.Object,
                _stubPasswordHasher.Object,
                _stubHttpContextAccessor.Object,
                _mapper
            );

            //Act
            var result = sut.AuthenticateAsync(auth);

            //Assert
            await result.ContinueWith(x => x.Status.Should().Be(TaskStatus.Faulted));
        }

        [Fact]
        public async void Should_not_authenticate_a_user_with_wrong_password()
        {
            //Arrange
            var user = new RegisterRequest { Email = "Test1", Password = "Test1" };
            var auth = new AuthenticateRequest { Email = "Test1", Password = "Test2" };
            var sut = new AccountService(
                _context,
                _stubJwtService.Object,
                _stubPasswordHasher.Object,
                _stubHttpContextAccessor.Object,
                _mapper
            );

            await sut.RegisterAsync(user);

            //Act
            var result = sut.AuthenticateAsync(auth);

            //Assert
            await result.ContinueWith(x => x.Status.Should().Be(TaskStatus.Faulted));
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}