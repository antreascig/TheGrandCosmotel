using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebGames.Libs;
using WebGames.Libs.Games;

namespace WebGames.Controllers
{
    [Authorize(Roles = "player,demo")]
    public class GamesController : Controller
    {
        #region Views
        public ActionResult ActiveGame()
        {
            var customgame = Request.QueryString["customgame"] ?? "";
            var activeGameInfo = GameManager.GetActiveGameInfo(User.Identity.GetUserId(), customgame);

            if (Request.QueryString["showdemo"] == "true")
            {
                activeGameInfo.IsDemo = true;
                activeGameInfo.GameScore = 0;
                activeGameInfo.RemainingTime = 1 * 60; // 1 minute
                activeGameInfo.ActiveLevel = 1;
            }

            return GetPage(activeGameInfo);
        }

        public ActionResult ActiveGameMap()
        {
            var customgame = Request.QueryString["customgame"] ?? "";
            var activeGameInfo = GameManager.GetActiveGameInfo(User.Identity.GetUserId(), customgame);

            return GetPage(activeGameInfo, "Map");
        }

        public ActionResult ActiveExplainer()
        {
            var customgame = Request.QueryString["customgame"] ?? "";
            var activeGameInfo = GameManager.GetActiveGameInfo(User.Identity.GetUserId(), customgame);

            return GetPage(activeGameInfo, "Explainer");
        }

        public ActionResult ActiveGameAfter()
        {
            var status = Request.QueryString["status"] ?? "";

            var customgame = Request.QueryString["customgame"] ?? "";
            var activeGameInfo = GameManager.GetActiveGameInfo(User.Identity.GetUserId(), customgame);

            if (activeGameInfo.ActiveGameDataModel != null)
            {
                var message = activeGameInfo.ActiveGameDataModel.Messages.ContainsKey(status) ? activeGameInfo.ActiveGameDataModel.Messages[status] : "";
                return View("ActiveGameAfter", new Dictionary<string, string>() { { "message", message } });
            }
            else
            {
                return View("NoActiveGame");
            }
        }

        //public ActionResult Group()
        //{
        //    var UserId = User.Identity.GetUserId();

        //    var UserName = User.Identity.GetUserName();

        //    var UserGroup = Group_Manager.GetUserTeam(UserId);

        //    if (UserGroup != null)
        //    {
        //        UserGroup.UsersInGroup = (UserGroup.UsersInGroup ?? new List<string>()).Where(u => u != UserName).ToList();
        //    }
        //    //UserGroup = null;

        //    return View("Group", UserGroup);
        //}
        #endregion

        #region Game3

        public ActionResult Get_Random_Game3_Solution()
        {
            var Rnd = new Random(DateTime.UtcNow.Second);

            var res = new int[4];
            for (int i = 0; i < res.Length; i++)
            {
                var num = 0;
                do
                {
                    num = Rnd.Next(1, 7);
                } while (!res.Contains(num));
                res[i] = num;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Game5

        //public ActionResult CheckQuestion(int questionId, int answerIndex)
        //{
        //    var isDemoStr = Request.QueryString["isDemo"] ?? "false";
        //    var isDemo = false;
        //    bool.TryParse(isDemoStr, out isDemo);
        //    var customGameKey = Request.QueryString["customGameKey"] ?? Request.QueryString["customgame"] ?? "";
        //    var UserId = User.Identity.GetUserId();
        //    // Security - Check if Game is the currently active one - cannot set the score for a non active game
        //    var ActiveGameDataModel = GameManager.GetActiveGameInfo(UserId, customGameKey).ActiveGameDataModel;
        //    if (!isDemo && (ActiveGameDataModel== null || ActiveGameDataModel.ActiveGameKey != GameKeys.Questions))
        //    {
        //        return Json(new { success = false, message = "Game is not active" }, JsonRequestBehavior.AllowGet);
        //    }
        //    var res = Questions_Manager.CheckAndSaveQuestionAnswer(UserId, questionId, answerIndex); // Cannot override the score - once is set the done
        //    return Json(new { success = true, isCorrect = res.IsCorrect, correctAnswer = res.CorrectAnswer }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetPlayerQuestions()
        //{
        //    var res = new List<object>();
        //    try
        //    {
        //        res.AddRange(Questions_Manager.GetPlayerQuestions(User.Identity.GetUserId()).Select(q => new
        //        {
        //            id = q.QuestionId,
        //            question = q.QuestionText,
        //            answer1 = q.Options[0],
        //            answer2 = q.Options[1],
        //            answer3 = q.Options[2],
        //            answer4 = q.Options[3],
        //        }));
        //    }
        //    catch (Exception exc)
        //    {
        //        Logger.Log(exc);
        //    }
        //    return Json(res);
        //}

        #endregion

        public ActionResult Save_Game_Score(int score, long timeStamp, int? level = 1)
        {
            var UserId = User.Identity.GetUserId();

            var customGameKey = Request.QueryString["customGameKey"] ?? Request.QueryString["customgame"] ?? "";
            var ActiveGameKey = "";
            if (customGameKey != "")
            {
                ActiveGameKey = customGameKey;
            }
            else
            {
                var ActiveGameData = GameManager.GetActiveGameInfo(UserId, null).ActiveGameDataModel;
                if (ActiveGameKey != null)
                {
                    ActiveGameKey = ActiveGameData.ActiveGameKey;
                }
            }
            // Security - Check if Game is the currently active one - cannot set the score for a non active game
            if (ActiveGameKey == "" || !GameManager.GameDict.ContainsKey(ActiveGameKey))
            {
                return Json(new { success = false, message = "No Game is Active" }, JsonRequestBehavior.AllowGet);
            }

            GameManager.GameDict[ActiveGameKey].SM.SetUserScore(UserId, score, timeStamp, level.GetValueOrDefault(), EnableOverride: false); // Cannot override the score - once is set the done

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveGameTime(int remainingTime, long timestamp)
        {
            var UserId = User.Identity.GetUserId();
            ActivityManager.SyncPlayedTimeForToday(UserId, remainingTime, timestamp);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGameTime()
        {
            var UserId = User.Identity.GetUserId();
            var res = UserGameManager.GetUserRemainingTime(User.Identity.GetUserId());
            return Json(new { success = true, time = res }, JsonRequestBehavior.AllowGet);
        }

        // Helpers //
        private ActionResult GetPage(ActiveUserGameInfo gameInfo, string Page = "")
        {
            ActionResult ViewRes = null;
            // Active game was found
            if (gameInfo.ActiveGameDataModel != null)
            {
                // No remaining time
                if (gameInfo.RemainingTime <= 0)
                {
                    ViewRes = RedirectToAction("ActiveGameAfter", new { status = "outoftime" });
                } // no available levels
                else if (gameInfo.AvailableLevels > 0 && gameInfo.AvailableLevels < gameInfo.ActiveLevel)
                {
                    ViewRes = RedirectToAction("ActiveGameAfter", new { status = "success" });
                }
                else
                {
                    string ViewPageToDisplay = gameInfo.Folder;
                    if (Page != "")
                    {
                        ViewPageToDisplay += $"/{Page}";
                    }
                    else // it's the game so use the PageFolder for the file
                    {
                        ViewPageToDisplay += $"/{gameInfo.Page}";

                        // If level is on its own page
                        if (gameInfo.LevelAsPage)
                        {
                            // use level specific page
                            ViewPageToDisplay += $"-{gameInfo.ActiveLevel}";
                        }
                    }

                    ViewRes = View(ViewPageToDisplay, gameInfo);
                }
            }
            else
            {
                // Nothing was found
                ViewRes = View("NoActiveGame");
            }

            return ViewRes;
        }
    }
}