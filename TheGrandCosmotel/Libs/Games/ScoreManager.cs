using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.JQuery.DataTables;
using WebGames.Models;

namespace WebGames.Libs.Games
{
    public class UserAllScores
    {
        public Dictionary<string, double> GameScoreDict { get; set; }
    }

    public class UserScore
    {
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string UserId { get; set; }
        public double Score { get; set; }
        public int Levels { get; set; }
    }

    public class UserScore_Admin : UserScore
    {
        public int Tokens { get; set; }
    }

    public class UserScoreVM : UserScore
    {
        public string User_FullName { get; set; }
    }

    public class UserTotalScore
    {
        public string UserId { get; set; }
        public string User_FullName { get; set; }
        public string User_Name { get; set; }
        public double Score { get; set; }
    }

    public abstract class ScoreManager
    {
        //public static void SetGenericScoreTokens(string GameKey, string UserId, int Tokens, bool Override = false)
        //{
        //    if (!GameManager.GameDict.ContainsKey(GameKey)) return;
        //    var Game = GameManager.GameDict[GameKey];

        //    Game.SM.SetUserScore(UserId, Tokens, Override);
        //}

        public static Dictionary<string, UserScore_Admin> GetUserTotalScores(string UserId)
        {
            var res = new Dictionary<string, UserScore_Admin>();
            using (var db = ApplicationDbContext.Create())
            {
                var User = (from u in db.Users where u.Id == UserId select u).SingleOrDefault();
                if (User != null)
                {
                    foreach (var game in GameManager.GameDict)
                    {
                        res.Add(game.Key, new UserScore_Admin()
                        {
                            UserId = UserId,
                            GameId = game.Value.GameId,
                            GameName = game.Value.Name,
                            Score = 0,
                            Tokens = 0,
                            Levels = 0
                        });
                        var Game1Score = game.Value.SM.GetUserScore(UserId, User);
                        if (Game1Score != null)
                        {
                            res[game.Key].Score = Game1Score.Score;
                            res[game.Key].Tokens = Game1Score.Tokens;
                            res[game.Key].Levels = Game1Score.Levels;
                        }
                    }
                }
            }
            return res;
        }

        public static List<UserTotalScore> GetUsersTotalScoresForGames(IEnumerable<string> selectedGameKeys, string[] UserIds = null )
        {
            var res = new List<UserTotalScore>();

            using (var db = ApplicationDbContext.Create())
            {
                var Games = (from game in db.Games select game).ToList();
                var q = db.Users.AsQueryable();
                if (UserIds != null)
                {
                    q = q.Where(user => UserIds.Contains(user.Id)).AsQueryable();
                }
                var Users = q.Select(user => new { UserId = user.Id, Full_Name = user.FullName, UserName = user.UserName }).ToList();
                var ScoreDict = Users.ToDictionary(k => k.UserId, v => (double)0);

                foreach( var gameKey in selectedGameKeys)
                {
                    var Game = Games.FirstOrDefault(g => g.GameKey == gameKey);
                    if (!GameManager.GameDict.ContainsKey(gameKey) || Game == null) continue;

                    var Game_Scores = GameManager.GameDict[gameKey].SM.GetUsersScore().ToList();

                    Game_Scores.ForEach(sc =>
                    {
                        if (ScoreDict.ContainsKey(sc.UserId))
                        {
                            ScoreDict[sc.UserId] += CalculateScore(Game, sc.Tokens);
                        }
                    });
                }
                res.AddRange(Users.Select(u => new UserTotalScore()
                {
                    UserId = u.UserId,
                    User_FullName = u.Full_Name,
                    Score = ScoreDict[u.UserId],
                    User_Name = u.UserName
                }));
            }
            return res;
        }

        public abstract void SetUserScore(string UserId, int Tokens, long timeStamp, int level, bool EnableOverride = false);

        public abstract DataTablesResult GetUsersScoresDT(DataTablesParam dataTableParam);

        public abstract UserScore_Admin GetUserScore(string UserId, ApplicationUser User = null);

        public abstract List<A_UserScore> GetUsersScore();

        // HELPERS
        public static UserScore_Admin GenerateUserScore(GameModel Game, ApplicationUser User, int Tokens, int Levels)
        {
            var res = new UserScore_Admin()
            {
                GameId = Game.GameId,
                GameName = Game.Name,
                UserId = User.Id,
                Tokens = Tokens,
                Score = CalculateScore(Game, Tokens),
                Levels = Levels
            };
            return res;
        }

        public static double CalculateScore(GameModel Game, int tokens)
        {
            return Game.Multiplier * tokens;
        }
    }

    public class ScoreManager<T> : ScoreManager where T : A_UserScore, new()
    {
        string GameKey { get; set; }
        public ScoreManager(string GK)
        {
            GameKey = GK;
        }

        public override UserScore_Admin GetUserScore(string UserId, ApplicationUser User = null)
        {
            try
            {
                var GameData = GameManager.GameDict[GameKey];
                using (var db = ApplicationDbContext.Create())
                {
                    if (User == null)
                    {
                        User = (from user in db.Users where user.Id == UserId select user).Single();
                    }

                    var Game = (from game in db.Games where game.GameId == GameData.GameId select game).SingleOrDefault();

                    var data = (from score in db.Set<T>() where score.UserId == UserId select score).SingleOrDefault();

                    var Tokens = 0;
                    var Levels = 0;
                    if (data != null)
                    {
                        Tokens = data.Tokens;
                        Levels = data.Levels;
                    }

                    var res = GenerateUserScore(Game, User, Tokens, Levels);
                    return res;
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw exc;
            }
        }

        public override DataTablesResult GetUsersScoresDT(DataTablesParam dataTableParam)
        {
            try
            {
                var GameData = GameManager.GameDict[GameKey];
                using (var db = ApplicationDbContext.Create())
                {
                    var Game = (from game in db.Games where game.GameId == GameData.GameId select game).SingleOrDefault();
                    var counter = 1;
                    var data = db.Set<T>().Join(db.Users, score => score.UserId, u => u.Id, (i, o) => new
                    {
                        UserId = i.UserId,
                        Name = o.FullName,
                        Tokens = i.Tokens,
                        Shop = o.Shop
                    }).OrderByDescending(s => s.Tokens).ToList().Select(i => new
                    {
                        Rank = counter++,
                        UserId = i.UserId,
                        Name = i.Name,
                        Shop = i.Shop,
                        Tokens = i.Tokens,
                        Score = 0
                    }).ToList().AsQueryable();

                    //var ScoreData = db.Set<T>().Include("User").AsQueryable();
                    var res = DataTablesResult.Create(data, dataTableParam, row => new
                    {
                        Rank = row.Rank,
                        UserId = row.UserId,
                        Name = row.Name,
                        Shop = row.Shop,
                        Tokens = row.Tokens,
                        Score = CalculateScore(Game, row.Tokens)
                    });
                    return res;
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw exc;
            }
        }

        public override List<A_UserScore> GetUsersScore()
        {
            using (var db = ApplicationDbContext.Create())
            {
                var data = db.Set<T>().ToList().Select(d => d as A_UserScore).ToList();
                return data;
            }
        }

        public override void SetUserScore(string UserId, int Tokens, long timeStamp, int level, bool EnableOverride = false)
        {
            try
            {
                using (var db = ApplicationDbContext.Create())
                {

                    var Entity = (from score in db.Set<T>() where score.UserId == UserId select score).SingleOrDefault();
                    if (Entity == null)
                    {
                        Entity = new T()
                        {
                            UserId = UserId,
                            Tokens = Tokens,
                            Levels = level,
                            timeStamp = timeStamp
                        };
                        db.Set<T>().Add(Entity);
                    }
                    else if (EnableOverride)
                    {
                        Entity.Tokens = Tokens;
                        Entity.timeStamp = timeStamp;
                    }
                    else if (Entity.timeStamp < timeStamp)
                    {
                        Entity.Tokens = Tokens;
                        Entity.Levels = level;
                        Entity.timeStamp = timeStamp;
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
            }
        }
    }
}