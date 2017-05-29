using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;
using WebGames.Helpers;
using WebGames.Libs.Games;
using WebGames.Models.ViewModels;
using WebGames.Libs.Games.Games;

namespace WebGames.Libs
{
    public class GameData
    {
        public int GameId { get; set; }
        public string GameKey{ get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Page { get; set; }
        public bool LevelAsPage = false;
        public int AvailableLevels { get; set; }
        public ScoreManager SM { get; set; }
    }

    public class GameKeys
    {
        //public const string Adespotabalakia = "adespotabalakia";
        //public const string Juggler = "juggler";
        //public const string Mastermind = "mastermind";
        //public const string Escape_1 = "escape_1";
        //public const string Escape_2 = "escape_2";
        //public const string Escape_3 = "escape_3";
        //public const string Whackamole = "whackamole";
        //public const string Questions = "questions";
    }

    public class ActiveUserGameInfo
    {
        public ActiveDailyData ActiveGameDataModel { get; set; }

        public string Folder { get; set; }

        public string Page { get; set; }

        public int RemainingTime { get; set; }

        public double GameScore { get; set; }

        public bool LevelAsPage { get; set; }

        public int ActiveLevel{ get; set; }

        public int AvailableLevels { get; set; }

        public bool IsDemo { get; set; }
    }

    public class ActiveDailyData
    {
        public string ActiveGameKey { get; set; }

        public Dictionary<string, string> Messages { get; set; }
    }

    public class GameManager
    {
        public static Dictionary<string, GameData> GameDict = new Dictionary<string, GameData>(); // < GameKey, GameId >

        static GameManager()
        {
            var Games = new List<GameData>()
            {
                //new GameData() { GameKey = GameKeys.Adespotabalakia, Name = "ΑΔΕΣΠΟΤΑ ΜΠΑΛΑΚΙΑ", Folder = "adespotabalakia", Page = "adespotabalakia", SM = new ScoreManager<Adespotabalakia_UserScore>(GameKeys.Adespotabalakia) },
                //new GameData() { GameKey = GameKeys.Escape_1, Name = "ΚΛΟΥΒΙΑ ΚΛΟΥΒΙΑ 1", Folder = "escape", Page = "game1", SM = new ScoreManager<Escape_1_UserScore>(GameKeys.Escape_1), LevelAsPage = true, AvailableLevels = 5 },
                //new GameData() { GameKey = GameKeys.Escape_2, Name = "ΚΛΟΥΒΙΑ ΚΛΟΥΒΙΑ 2", Folder = "escape", Page = "game2", SM = new ScoreManager<Escape_2_UserScore>(GameKeys.Escape_2), LevelAsPage = true, AvailableLevels = 5 },
                //new GameData() { GameKey = GameKeys.Escape_3, Name = "ΚΛΟΥΒΙΑ ΚΛΟΥΒΙΑ 3", Folder = "escape", Page = "game3", SM = new ScoreManager<Escape_3_UserScore>(GameKeys.Escape_3), LevelAsPage = true, AvailableLevels = 5 },
                //new GameData() { GameKey = GameKeys.Mastermind, Name = "ΜΑΝΤΕΨΕ ΤΙ ΜΑΝΤΕΨΑ", Folder = "mastermind", Page = "mastermind", SM = new ScoreManager<Mastermind_UserScore>(GameKeys.Mastermind) },
                //new GameData() { GameKey = GameKeys.Juggler, Name = "ΑΤΕΛΕΙΩΤΟ ΣΚΟΙΝΑΚΙ", Folder = "juggler", Page = "juggler", SM = new ScoreManager<Juggler_UserScore>(GameKeys.Juggler) },
                //new GameData() { GameKey = GameKeys.Whackamole, Name = "WHACK A MOLE", Folder = "whackamole",  Page = "whackamole", SM = new ScoreManager<Whackamole_UserScore>(GameKeys.Whackamole) },
                //new GameData() { GameKey = GameKeys.Questions, Name = "ΚΛΕΙΔΙΑ", Folder = "questions", Page = "questions", SM = new ScoreManager<Questions_UserScore>(GameKeys.Questions) },
            };
            using (var db = ApplicationDbContext.Create())
            {
                var ExistingGames = db.Games.ToList().ToDictionary(k => k.GameKey);
                foreach (var game in Games)
                {
                    if (ExistingGames.ContainsKey(game.GameKey))
                    {
                        game.GameId = ExistingGames[game.GameKey].GameId;
                        if (ExistingGames[game.GameKey].Name != game.Name)
                        {
                            ExistingGames[game.GameKey].Name = game.Name;
                        }
                    }
                    else
                    {
                        var GameModel = new GameModel()
                        {
                            GameKey = game.GameKey,
                            Name = game.Name,
                        };
                        db.Games.Add(GameModel);
                        game.GameId = GameModel.GameId;
                    }
                    GameDict.Add(game.GameKey, game);
                }
                // Remove obsolete games
                ExistingGames.Values.Where(eg => !Games.Any(g => g.GameKey == eg.GameKey)).ToList().ForEach(g => db.Games.Remove(g));

                db.SaveChanges();

            }
        }

        public static ActiveUserGameInfo GetActiveGameInfo(string UserId, string CustomGameKey)
        {
            var data = new ActiveUserGameInfo()
            {
                //ActiveGameData = new ActiveGameData() { ActiveGameKey = "", Messages = new Dictionary<string, string>() },
                RemainingTime = 0,
                GameScore = 0,
                LevelAsPage = false,
                ActiveLevel = 1,
                AvailableLevels = 0
            };

            // Check for active game data
            if ((CustomGameKey ?? "") != "")
            {
                data.ActiveGameDataModel = new ActiveDailyData()
                {
                    ActiveGameKey = CustomGameKey,
                    Messages = new Dictionary<string, string>()
                    {
                        { "success", "Μπράβο!" },
                        { "fail", "Έχασες!"},
                        { "outoftime", "Τέλος χρόνου"}
                    }
                };
            }
            else
            {
                data.ActiveGameDataModel = GameDayScheduleManager.GetActiveDailyData(DateTime.UtcNow);
            }
            if (data.ActiveGameDataModel != null && !GameDict.ContainsKey(data.ActiveGameDataModel.ActiveGameKey))
            {
                data.ActiveGameDataModel = null;
            }

            if (data.ActiveGameDataModel == null)
            {
                return data;
            }

            // Check for remaining time
            data.RemainingTime = UserGameManager.GetUserRemainingTime(UserId).RemainingTimeInSeconds;

            var ActiveGameKey = data.ActiveGameDataModel.ActiveGameKey;

            var gameData = GameManager.GameDict[ActiveGameKey];
            // Get Page/Folder
            data.Folder = gameData.Folder;
            data.Page = gameData.Page;

            // Get score for the game
            data.GameScore = gameData.SM.GetUserScore(UserId).Score;
            data.ActiveLevel = gameData.SM.GetUserScore(UserId).Levels + 1;
            data.AvailableLevels = gameData.AvailableLevels;
            // Check if level is showed as different page
            data.LevelAsPage = GameDict[ActiveGameKey].LevelAsPage;

            // If Group game check if user is allowed to play it
            //if (Group_Manager.GroupGames.Contains( ActiveGameKey))
            //{
            //    var Group = Group_Manager.GetUserGroup(UserId);
            //    if (Group == -1)
            //    {
            //        data.ActiveGameDataModel = null;
            //    }
            //}

            return data;
        }

        public static List<GameSettings> GetGameSettings()
        {
            var settings = new List<GameSettings>();
            try
            {
                using (var db = ApplicationDbContext.Create())
                {
                    settings.AddRange((from game in db.Games select game)
                                        .Select(g => new GameSettings()
                                        {
                                            GameId = g.GameId,
                                            GameKey = g.GameKey,
                                            Name = g.Name,
                                            Multiplier = g.Multiplier
                                        }).ToList().Where(g => GameManager.GameDict.ContainsKey(g.GameKey)));
                }
            }
            catch(Exception exc)
            {
                Logger.Log(exc);
            }
            
            return settings;
        }

        public static void SetGameSettings(GameSettings model )
        {
            if (model == null) return;
            try
            {
                var GameId = GameManager.GameDict[model.GameKey].GameId;
                using (var db = ApplicationDbContext.Create())
                {
                    var Game = GameHelper.GetGame(GameId, db);
                    if (Game == null) throw new ArgumentNullException($"Game {model.Name} - {model.GameKey} does not exist");
                    // Copy the settings to the model
                    Game.Multiplier = model.Multiplier;
                    db.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw exc;
            }

        }        
    }
}