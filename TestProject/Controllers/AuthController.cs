using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;
using TestProject.Models.Request;
using TestProject.Models.Response;
using TestProject.Services.Authorization;
using TestProject.Services.Authorization.Models;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(ApplicationContext context, IJwtService jwtService, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (await _context.Users.AnyAsync(x => x.Email == model.Email))
            {
                return BadRequest("User with such Email exists");
            }

            var newUser = new User()
            {
                Lastname = model.Lastname,
                Firstname = model.Firstname,
                Email = model.Email,
                Password = model.Password,
            };

            newUser.Password = _passwordHasher.HashPassword(newUser, model.Password);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var token = _jwtService.GetToken(new JwtUser { UserId = newUser.Id.ToString(), Role = newUser.Role.ToString() });

            return Ok(new AuthorizeResponse
            {
                Token = token,
                UserId = newUser.Id,
                Role = newUser.Role.ToString()
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
            {
                return BadRequest("Email or password is incorrect");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest("Email or password is incorrect");
            }
            else if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.Password = _passwordHasher.HashPassword(user, model.Password);
                await _context.SaveChangesAsync();
            }

            var token = _jwtService.GetToken(new JwtUser { UserId = user.Id.ToString(), Role = user.Role.ToString() });

            return Ok(new AuthorizeResponse
            {
                Token = token,
                Role = user.Role.ToString(),
                UserId = user.Id
            });
        }
    }
}
