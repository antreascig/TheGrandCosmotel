using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebGames.Models
{
    public class User_Group
    {
        [Key]
        public string UserId { get; set; }

        public int GroupNumber { get; set; }

        public string User_FullName { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}