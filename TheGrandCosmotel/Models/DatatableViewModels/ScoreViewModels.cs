using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Models.DatatableViewModels
{
    public class UserScoreViewModel
    {
        public string UserId { get; set; }
        public int Rank { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Shop { get; set; }
        public string Controls { get; set; }
    }

    public class GroupsViewModel
    {
        public int GroupNumber { get; set; }
        public double Score { get; set; }
        public string Controls { get; set; }
    }

    public class GroupViewModel
    {
        public int GroupNumber { get; set; }
        public string UserId { get; set; }
        public string User_FullName { get; set; }
        public double Score { get; set; }
        public string Controls { get; set; }
    }
}