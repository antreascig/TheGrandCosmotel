using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGames.Models
{
    public class DayActiveGame
    {
        [Key]
        public string Day { get; set; } // yyyy-mm-dd

        public string GameKey { get; set; }

        public string SuccessMessage { get; set; }

        public string FailMesssage { get; set; }

        public string OutOfTimeMessage { get; set; }

        public string InfoMessage { get; set; }
    }
}