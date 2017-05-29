//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using WebGames.Models;

//namespace WebGames.Libs.Games.GameTypes
//{
//    public class QuestionAnswer
//    {
//        public bool IsCorrect { get; set; }
//        public int CorrectAnswer { get; set; }
//    }

//    public class GameQuestionView
//    {
//        public int QuestionId { get; set; }
//        public string QuestionText { get; set; }
//        public List<string> Options { get; set; }
//    }

//    public class GameQuestionModel
//    {
//        public int QuestionId { get; set; }
//        public bool Active { get; set; }
//        public string QuestionText { get; set; }
//        public List<string> Options { get; set; }
//        public int AnswerIndex { get; set; }
//    }

//    public class Questions_MetaData
//    {
//        public int PointsPerQustion = 20;

//        public Dictionary<int, GameQuestionModel> Questions { get; set; }
//    }

//    public class Questions_UserScore_Dto
//    {
//        public int GameId { get; set; }
//        public string UserId { get; set; }
//        public string Answers { get; set; } // Dictionary<int, int>
//        public double Computed_Score { get; set; }
//    }

//    public class Questions_Manager
//    {
//        public static int GameId {
//            get
//            {
//                return GameManager.GameDict[GameKeys.Questions].GameId;
//            }
//        }

//        public static List<GameQuestionView> GetPlayerQuestions( string UserId )
//        {
//            var res = new List<GameQuestionView>();
//            var temp = new List<GameQuestionModel>();
//            var Ids = new List<int>();
//            var WasAssignedQuestions = false;
//            try
//            {
//                using (var db = ApplicationDbContext.Create())
//                {
//                    // Check if questions have already been assigned to the user
//                    var UserQuestionsModel = (from uq in db.User_Questions where uq.UserId == UserId select uq).SingleOrDefault();
//                    if (UserQuestionsModel != null && (UserQuestionsModel.Questions ?? "") != "")
//                    {
//                        var answered = (UserQuestionsModel.Answered ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
//                        Ids.AddRange(UserQuestionsModel.Questions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries )
//                                                                 // remove any already answered questions
//                                                                 .Where(id => !answered.Contains(id))
//                                                                 .Select(id => int.Parse(id)) );
//                        WasAssignedQuestions = true;
//                    }

//                    // Retrieve the game's metadata
//                    var GameMetadata = (Questions_MetaData)GameHelper.GetGameMetaData(GameId, typeof(Questions_MetaData));

//                    if (GameMetadata != null && GameMetadata.Questions != null)
//                    {
//                        // get the score of the user in order to calculate the required number of questions to retrieve
//                        //var scores = ScoreManager.GetUserTotalScores(UserId);
//                        //var GamesRequired = new string[] { GameKeys.Adespotabalakia, GameKeys.Juggler, GameKeys.Mastermind, GameKeys.Escape_1, GameKeys.Escape_2, GameKeys.Escape_3, GameKeys.Whackamole };
//                        //var totalScore = scores.Where(s=> GamesRequired.Contains(s.Key) ).Sum(s => s.Value.Score);
//                        //int NumberOfQuestions = (int)Math.Ceiling(totalScore / GameMetadata.PointsPerQustion);
//                        //int NumberOfQuestions = 400;
//                        // if not any ids stored in db them choose randomly
//                        if ( !WasAssignedQuestions )
//                        {
//                            var AllQIds = GameMetadata.Questions.Where(q => q.Value.Options.Count == 4 ).Select(q => q.Key).ToList();
//                            var NumberOfQuestions = AllQIds.Count;
//                            var RandomNumGen = new Random(DateTime.UtcNow.Millisecond);
//                            while (Ids.Count < NumberOfQuestions && NumberOfQuestions <= AllQIds.Count )
//                            {
//                                var randIndex = RandomNumGen.Next(0, AllQIds.Count);
//                                var idToAdd = AllQIds[randIndex];
//                                if ( !Ids.Contains(idToAdd) )
//                                {
//                                    Ids.Add(idToAdd);
//                                }
//                            }

//                            if (UserQuestionsModel == null)
//                            {
//                                db.User_Questions.Add(new UserQuestion()
//                                {
//                                    UserId = UserId,
//                                    Questions = string.Join(",", Ids)
//                                });
//                            }
//                            else
//                            {
//                                UserQuestionsModel.Questions = string.Join(",", Ids);
//                            }
                            
//                            db.SaveChanges();
//                        }
//                    }
//                    res.AddRange(
//                          GameMetadata.Questions.Where(q => q.Value.Active && Ids.Contains(q.Key))
//                                                .Select(q => new GameQuestionView()
//                                                {
//                                                    QuestionId = q.Value.QuestionId,
//                                                    QuestionText = q.Value.QuestionText,
//                                                    Options = q.Value.Options,
//                                                })
//                    );
//                }                
//            }
//            catch (Exception exc)
//            {
//                Logger.Log(exc.Message, LogType.ERROR);
//            }
//            return res;
//        }

//        //public static int Score_Limit { get; set; }
//        public static List<GameQuestionModel> GetQuestions()
//        {
//            var res = new List<GameQuestionModel>();
//            try
//            {
//                var GameMetadata = (Questions_MetaData)GameHelper.GetGameMetaData(GameId, typeof(Questions_MetaData));
//                if (GameMetadata != null && GameMetadata.Questions != null)
//                {
//                    res.AddRange(GameMetadata.Questions.Select(q => q.Value));
//                }
//            }
//            catch(Exception exc)
//            {
//                Logger.Log(exc.Message, LogType.ERROR);
//            }
//            return res;
//        }

//        public static void SaveQuestions(List<GameQuestionModel> Questions)
//        {
//            using (var db = ApplicationDbContext.Create())
//            {
//                var Game = (from game in db.Games where game.GameId == GameId select game).SingleOrDefault();

//                var CurrentMetadata = Newtonsoft.Json.JsonConvert.DeserializeObject<Questions_MetaData>(Game.MetadataJSON ?? "{}");

//                var md = new Questions_MetaData()
//                {
//                    Questions = Questions.ToDictionary(k => k.QuestionId),
//                    PointsPerQustion = CurrentMetadata.PointsPerQustion
//                };

//                Game.MetadataJSON = Newtonsoft.Json.JsonConvert.SerializeObject(md);
//                var res = db.SaveChanges();
//            }
//        }

//        public static QuestionAnswer CheckAndSaveQuestionAnswer(string UserId, int QuestionId, int AnswerIndex)
//        {
//            try
//            {
//                var IsCorrect = false;
//                int CorrectAnswer = -1;
//                int totalCorrect = 0;
//                using (var db = ApplicationDbContext.Create())
//                {
//                    var GameMetadata = (Questions_MetaData)GameHelper.GetGameMetaData(GameId, typeof(Questions_MetaData), db);
//                    // Check that is the correct question
//                    if (GameMetadata != null
//                        || GameMetadata.Questions != null
//                        || GameMetadata.Questions.ContainsKey(QuestionId)
//                        || GameMetadata.Questions[QuestionId].QuestionId == QuestionId )
//                    {
//                        CorrectAnswer = GameMetadata.Questions[QuestionId].AnswerIndex;

//                        IsCorrect = CorrectAnswer == AnswerIndex;
//                    }
//                    else
//                    {
//                        IsCorrect = false;
//                    }

//                    // Save question is as answered
//                    var User_Questions = (from q in db.User_Questions where q.UserId == UserId select q).SingleOrDefault();
//                    var Existing = (User_Questions.Answered ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
//                    Existing.Add(QuestionId.ToString());
//                    User_Questions.Answered = string.Join(",", Existing);
//                    if (IsCorrect)
//                    {
//                        User_Questions.Correct += 1;
//                    }
//                    totalCorrect = User_Questions.Correct;

//                    db.SaveChanges();
//                }

//                // save the score
//                long timeStamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1);

//                GameManager.GameDict[GameKeys.Questions].SM.SetUserScore(UserId, totalCorrect, timeStamp, 1, false);
                
//                return new QuestionAnswer() { IsCorrect = IsCorrect, CorrectAnswer = CorrectAnswer };
//            }
//            catch (Exception exc)
//            {
//                Logger.Log(exc.Message, LogType.ERROR);
//                return new QuestionAnswer() { IsCorrect = false, CorrectAnswer = -1 };
//            }
//        }
   
//    }
//}