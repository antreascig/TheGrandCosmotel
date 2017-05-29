using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebGames.Models
{
    public class GameModel
    {
        [Key]
        public int GameId { get; set; }

        public string GameKey { get; set; }

        public string Name { get; set; }

        public double Multiplier { get; set; }

        //public string MetadataJSON { get; set; } // JSON - Metadata of each game
    }
}