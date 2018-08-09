﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongerTogetherBLL.Model
{
    public class WrittingsBO
    {
        public long WrittingId { get; set; }

        public string Username { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime DatePublished { get; set; }

        [Required]
        [StringLength(5120)]
        public string Content { get; set; }

        public long UserId { get; set; }

        public int WordCount { get; set; }
    }
}
