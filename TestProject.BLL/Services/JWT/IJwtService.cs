using TestProject.BLL.Services.JWT.Models;

namespace TestProject.BLL.Services.JWT
{
    public interface IJwtService
    {
        public string GetToken(JwtUser user);
    }
}
