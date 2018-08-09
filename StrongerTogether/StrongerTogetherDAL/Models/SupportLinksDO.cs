using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongerTogetherDAL.Models
{
    public class SupportLinksDO
    {
        public long SupportId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Url { get; set; }

        public long UserId { get; set; }
    }
}
