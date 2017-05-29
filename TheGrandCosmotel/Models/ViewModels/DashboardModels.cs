using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Models.ViewModels
{
    public class DashBoardIndexModel
    {
        public int NumberOfPlayers { get; set; }

        public string ActiveGame { get; set; }

        public List<string> ScheduleDays { get; set; }  // matching indices
        public List<string> ScheduleGames { get; set; } // with this list
    }
}