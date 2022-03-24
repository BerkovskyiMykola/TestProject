using TestProject.Models;

namespace TestProject.Services.Authorization.Models
{
    public class JwtUser
    {
        public Guid Id { set; get; }
        public Role Role { set; get; }
    }
}
