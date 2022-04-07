using TestProject.DAL.Entities;
using TestProject.DTO.Request;
using TestProject.DTO.Response;

namespace TestProject.BLL.Services.ManageUser
{
    public interface IManageUserService
    {
        Task<IEnumerable<UserResponse>> GetUsersForAdminAsync();
        Task<UserResponse> CreateUserAsync(UserRequest model);
        Task EditUserAsync(Guid userId, EditUserRequest model);
        Task DeleteUserAsync(Guid userId);
    }
}
