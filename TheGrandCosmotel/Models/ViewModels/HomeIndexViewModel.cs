using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Libs;

namespace WebGames.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public bool IsWinner { get; set; }
        public ActiveDailyData DailyData { get; set; }
    }
}