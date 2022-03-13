using System.ComponentModel.DataAnnotations;

namespace TestProject.Models.Response
{
    public class AuthorizeResponse
    {
        public string Token { set; get; } = "";
        public string Role { set; get; } = "";
        public Guid UserId { set; get; }
    }
}
