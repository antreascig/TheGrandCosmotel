using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Helpers;
using WebGames.Libs.Games;
using WebGames.Models;
using WebGames.Models.ViewModels;

namespace WebGames.Libs
{
    public class UserGameManager
    {
        //const int TIME_LIMIT_PER_DAY = 20 * 60; // 300 seconds / 5 minutes

        public static UserGameInfo GetUserRemainingTime(string UserId)
        {
            var res = new UserGameInfo()
            {
                RemainingTimeInSeconds = 0,
                timeStamp = 0
            };

            var Today = DateHelper.GetGreekDate(DateTime.UtcNow, onlyDate: true);

            // Get Remaining Play time for today
            var gameTime = ActivityManager.GetGameTime(UserId, Today);
            if (gameTime != null)
            {
                var RemainingTime = ActivityManager.ALLOWED_TIME - gameTime.timeInSeconds;
                if (RemainingTime < 0) RemainingTime = 0;

                res.RemainingTimeInSeconds = RemainingTime;
                res.timeStamp = gameTime.timeStamp;
            }
            
            return res;
        }
    }
}