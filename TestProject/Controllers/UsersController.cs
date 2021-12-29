using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;
using TestProject.Models.Request;
using TestProject.Services.Authorization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TestProject.Services.Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IJwtService _jwtService;

        public UsersController(ApplicationContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
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
                Password = GetPasswordHash(model.Password),
                Role = await _context.Roles.SingleOrDefaultAsync(x => x.Name == "User")
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var token = _jwtService.GetToken(new JwtUser { Login = newUser.Id.ToString(), Role = newUser.Role!.Name });

            return Ok(new { token, UserId = newUser.Id, Role = newUser.Role.Name });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var user = await _context.Users.Include(x => x.Role).SingleOrDefaultAsync(x => x.Email == model.Email);

            if (user == null || user.Password != GetPasswordHash(model.Password))
            {
                return BadRequest("Email or password is incorrect");
            }

            var token = _jwtService.GetToken(new JwtUser { Login = user.Id.ToString(), Role = user.Role!.Name });
            var response = new
            {
                Token = token,
                Role = user.Role!.Name,
                UserId = user.Id
            };

            return Ok(response);
        }

        [HttpGet("one")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id.ToString() == HttpContext.User.Identity!.Name);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                user.Firstname,
                user.Lastname,
                user.Email,
                Role = user.Role!.Name
            });
        }

        [HttpPut("edit/profile/{id}")]
        [Authorize]
        public async Task<IActionResult> PutUserProfile(Guid id, ProfileRequest profile)
        {
            if (id != profile.UserId)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                return BadRequest();
            }

            user.Lastname = profile.Lastname;
            user.Firstname = profile.Firstname;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _context.Users
                .Include(x => x.Role)
                .Where(x => x.Email != HttpContext.User.Identity!.Name)
                .Select(x => new { x.Id, x.Firstname, x.Lastname, x.Email, x.Role })
                .ToListAsync());
        }

        private string GetPasswordHash(string password)
        {
            byte[] hash;
            using (var sha256 = SHA256.Create())
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }
}
