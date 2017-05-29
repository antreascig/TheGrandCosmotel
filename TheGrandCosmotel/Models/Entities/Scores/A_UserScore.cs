using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebGames.Models
{
    public abstract class A_UserScore
    {
        [Key]
        public string UserId { get; set; }

        public int Tokens { get; set; }

        public int Levels{ get; set; }

        public long timeStamp { get; set; }

        
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}