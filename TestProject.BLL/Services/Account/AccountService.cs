using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TestProject.BLL.Services.JWT;
using TestProject.BLL.Services.JWT.Models;
using TestProject.DAL.EF;
using TestProject.DAL.Entities;
using TestProject.DTO.Request;
using TestProject.DTO.Response;

namespace TestProject.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationContext _context;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(
            ApplicationContext context,
            IJwtService jwtService,
            IPasswordHasher<User> passwordHasher,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<AuthorizeResponse> AuthenticateAsync(AuthenticateRequest model)
        {
            await Task.Delay(1000);
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
            {
                throw new KeyNotFoundException("Email or password is incorrect");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new ArgumentException("Email or password is incorrect");
            }
            else if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.Password = _passwordHasher.HashPassword(user, model.Password);
                await _context.SaveChangesAsync();
            }

            var token = _jwtService.GetToken(_mapper.Map<JwtUser>(user));

            var response = _mapper.Map<AuthorizeResponse>(user);
            response.Token = token;

            return response;
        }

        public async Task EditUserProfile(ProfileRequest model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            if (userId == null) { throw new ArgumentNullException("HttpContext.User.Identity.Name is null"); }

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id.ToString() == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("No user found");
            }

            user.Lastname = model.Lastname;
            user.Firstname = model.Firstname;

            await _context.SaveChangesAsync();
        }

        public async Task<ProfileResponse> GetProfileInfoAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            if (userId == null) { throw new ArgumentNullException("HttpContext.User.Identity.Name is null"); }

            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Id.ToString() == userId);

            if (user == null)
            {
                throw new KeyNotFoundException("No user found");
            }

            return _mapper.Map<ProfileResponse>(user);
        }

        public async Task<AuthorizeResponse> RegisterAsync(RegisterRequest model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (await _context.Users.AnyAsync(x => x.Email == model.Email))
            {
                throw new ArgumentException("User with such Email exists");
            }

            var newUser = _mapper.Map<User>(model);

            newUser.Password = _passwordHasher.HashPassword(newUser, model.Password);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var token = _jwtService.GetToken(_mapper.Map<JwtUser>(newUser));

            var response = _mapper.Map<AuthorizeResponse>(newUser);
            response.Token = token;

            return response;
        }
    }
}
