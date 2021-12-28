using TestProject.Services.Authorization.Models;

namespace TestProject.Services.Authorization
{
    public interface IJwtService
    {
        public string GetToken(JwtUser user);
    }
}
