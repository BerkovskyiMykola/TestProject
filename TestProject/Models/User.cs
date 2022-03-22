using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Lastname { set; get; } = "";
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Firstname { set; get; } = "";
        [Required]
        [EmailAddress]
        public string Email { set; get; } = "";
        [Required]
        public string Password { set; get; } = "";

        public Role Role { get; set; } = Role.User;
    }
}
