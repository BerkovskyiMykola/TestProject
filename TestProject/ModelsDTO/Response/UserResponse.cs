using TestProject.Models;

namespace TestProject.ModelsDTO.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Lastname { set; get; } = "";
        public string Firstname { set; get; } = "";
        public string Email { set; get; } = "";
        public Role Role { get; set; }
    }
}
