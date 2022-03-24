using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;
using TestProject.ModelsDTO.Request;
using TestProject.Services.Authorization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using TestProject.ModelsDTO.Response;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public UsersController(ApplicationContext context, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Id.ToString() == HttpContext.User.Identity!.Name);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProfileResponse>(user));
        }

        [HttpPut("edit/profile")]
        [Authorize]
        public async Task<IActionResult> PutUserProfile(ProfileRequest profile)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id.ToString() == HttpContext.User.Identity!.Name);

            if (user == null)
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
            var users = await _context.Users
                .Where(x => x.Id.ToString() != HttpContext.User.Identity!.Name)
                .ToListAsync();

            return Ok(_mapper.Map<List<User>,List<UserResponse>>(users));
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostUser(User model)
        {
            if (await _context.Users.AnyAsync(x => x.Email == model.Email))
            {
                return BadRequest("User with such Email exists");
            }

            model.Password = _passwordHasher.HashPassword(model, model.Password);

            await _context.Users.AddAsync(model);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<UserResponse>(model));
        }

        [HttpPut("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUser(Guid id, EditUserRequest model)
        {
            if (id != model.UserId)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(model.UserId);

            if (user == null)
            {
                return NotFound();
            }

            user.Lastname = model.Lastname;
            user.Firstname = model.Firstname;
            user.Role = model.Role;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
