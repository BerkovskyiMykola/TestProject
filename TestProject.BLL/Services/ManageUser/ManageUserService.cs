using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TestProject.DAL.EF;
using TestProject.DAL.Entities;
using TestProject.DTO.Request;
using TestProject.DTO.Response;

namespace TestProject.BLL.Services.ManageUser
{
    public class ManageUserService : IManageUserService
    {
        private readonly ApplicationContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ManageUserService(
            ApplicationContext context, 
            IPasswordHasher<User> passwordHasher, 
            IHttpContextAccessor httpContextAccessor, 
            IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<UserResponse> CreateUserAsync(UserRequest model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (await _context.Users.AnyAsync(x => x.Email == model.Email))
            {
                throw new ArgumentException("User with such Email exists");
            }

            var user = _mapper.Map<User>(model);

            user.Password = _passwordHasher.HashPassword(user, user.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserResponse>(user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("No user found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task EditUserAsync(Guid userId, EditUserRequest model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (userId != model.UserId) { throw new ArgumentException("UserId is not same in model and Guid"); }

            var user = await _context.Users.FindAsync(model.UserId);

            if (user == null)
            {
                throw new KeyNotFoundException("No user found");
            }

            user.Lastname = model.Lastname;
            user.Firstname = model.Firstname;
            user.Role = model.Role;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserResponse>> GetUsersForAdminAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            if (userId == null) { throw new ArgumentNullException("HttpContext.User.Identity.Name is null"); }

            var users = await _context.Users
                .Where(x => x.Id.ToString() != userId)
                .ToListAsync();

            return _mapper.Map<List<User>, IEnumerable<UserResponse>>(users);
        }
    }
}
