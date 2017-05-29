using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebGames.Models
{
    public class UserQuestion
    {
        [Key]
        public string UserId { get; set; }

        public string Questions { get; set; } // "1,10,23,...etc"

        public string Answered { get; set; } // "1,10,...etc"

        public int Correct { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}