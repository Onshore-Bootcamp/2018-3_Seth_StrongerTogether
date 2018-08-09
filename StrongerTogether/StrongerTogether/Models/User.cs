using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StrongerTogether.Models
{
    public class User
    {
        public long UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,10}$", ErrorMessage ="Must be between 4 - 10 characters. Must have atleast one capital letter and atleast one number.")]
        public string Password { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public long RoleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }
    }
}