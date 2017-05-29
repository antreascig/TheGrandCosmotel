using WebGames.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebGames.Models
{
    public class UserDailyActivity
    {
        [Column(Order = 0), Key]
        public string UserId { get; set; }

        [Column(TypeName = "DateTime2", Order = 1), Key]
        public DateTime Date { get; set; } // format year/month/day

        public int TimePlayed { get; set; }

        public long Timestamp { get; set; }
        
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}