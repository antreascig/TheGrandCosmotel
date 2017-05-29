using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Models.ViewModels
{
    public class GameSettings
    {
        public int GameId { get; set; }

        public string GameKey { get; set; }

        public string Name { get; set; }

        public double Multiplier { get; set; }
    }
}