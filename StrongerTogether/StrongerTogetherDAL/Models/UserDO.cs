using System.ComponentModel.DataAnnotations;

namespace StrongerTogetherDAL.Models
{
    public class UserDO
     {
        public long UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public long RoleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }
    }
}
