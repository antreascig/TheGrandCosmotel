using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games.Games
{
    public class UserGroupVM
    {
        public int Rank { get; set; }
        public string UserId { get; set; }
        public string User_FullName { get; set; }
        public int Group { get; set; }
        public double Score { get; set; }
        public string Controls { get; set; }
    }

    public class Group_Team
    {
        public int Group { get; set; }
        public List<string> UsersInGroup { get; set; }
    }

    public class GroupScore
    {
        public int Group { get; set; }
        public double TotalScore
        {
            get
            {
                return (UsersInGroup ?? new List<UserTotalScore>()).Sum(u => u.Score);
            }
        }
        public List<UserTotalScore> UsersInGroup { get; set; }
    }

    public class Rank_Manager
    {
        public static List<UserGroupVM> GetRankings()
        {
            var res = new List<UserGroupVM>();

            var AtomicGames = GameManager.GameDict.Keys.ToArray();// new string[] { GameKeys.Adespotabalakia, GameKeys.Juggler, GameKeys.Mastermind, GameKeys.Escape_1, GameKeys.Escape_2, GameKeys.Escape_3 };
            var UserScores = ScoreManager.GetUsersTotalScoresForGames(AtomicGames);

            var TopUserScores = UserScores.OrderByDescending(s => s.Score).ToList();

            var groupFix = 0;
            for (var i = 0; i < TopUserScores.Count; i++)
            {             
                res.Add(new UserGroupVM()
                {
                    Rank = i + 1,
                    UserId = TopUserScores[i].UserId,
                    User_FullName = TopUserScores[i].User_FullName,
                    Group = (int)(i % 12) + 1 + groupFix * 12,
                    Score = TopUserScores[i].Score
                });

                if ((i + 1) % 144 == 0)
                {
                    groupFix++;
                }
            }
            return res;
        }
    }
}