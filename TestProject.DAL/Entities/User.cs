using System.ComponentModel.DataAnnotations;

namespace TestProject.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Lastname { set; get; } = string.Empty;
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Firstname { set; get; } = string.Empty;
        [Required]
        public string Email { set; get; } = string.Empty;
        [Required]
        public string Password { set; get; } = string.Empty;

        public Role Role { get; set; } = Role.User;
    }
}
