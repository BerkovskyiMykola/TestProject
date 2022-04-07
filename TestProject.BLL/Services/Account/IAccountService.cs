using TestProject.DTO.Request;
using TestProject.DTO.Response;

namespace TestProject.BLL.Services.Account
{
    public interface IAccountService
    {
        Task<AuthorizeResponse> RegisterAsync(RegisterRequest model);
        Task<AuthorizeResponse> AuthenticateAsync(AuthenticateRequest model);
        Task<ProfileResponse> GetProfileInfoAsync();
        Task EditUserProfile(ProfileRequest model);
    }
}
