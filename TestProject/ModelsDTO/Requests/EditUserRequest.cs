﻿using System.ComponentModel.DataAnnotations;

namespace TestProject.Models.Request
{
    public class EditUserRequest
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Lastname { set; get; } = "";
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Firstname { set; get; } = "";
        [Required]
        public Guid RoleId { set; get; }
    }
}
