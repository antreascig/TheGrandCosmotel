using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Helpers;
using WebGames.Models;

namespace WebGames.Libs.Games
{
    public class DayActiveGameView
    {
        public string Day { get; set; } // yyyy-mm-dd

        public string GameKey { get; set; }

        public string GameName { get; set; }

        public string SuccessMessage { get; set; }

        public string FailMesssage { get; set; }

        public string OutOfTimeMessage { get; set; }
    }

    public class GameDayScheduleManager
    {
        public static void SaveSchedule(List<DayActiveGame> Schedule)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var CurrentSchedule = (from ag in db.DailyActiveData select ag).ToList().ToDictionary(k => k.Day);
                foreach (var daySchedule in Schedule)
                {
                    // Make sure the day is in correct format
                    var parts = (daySchedule.Day ?? "").Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 3) throw new ArgumentException("Invalid Day Format");
                    int year, month, day;
                    if (!int.TryParse(parts[0], out year) || !int.TryParse(parts[1], out month) || !int.TryParse(parts[2], out day)) throw new ArgumentException("Invalid Day Format");
                    var Date = new DateTime(year, month, day);
                    // If the day is already in the db
                    if (CurrentSchedule.ContainsKey(daySchedule.Day) )
                    {
                        // change the gamekey
                        CurrentSchedule[daySchedule.Day].GameKey = daySchedule.GameKey;
                        CurrentSchedule[daySchedule.Day].SuccessMessage = daySchedule.SuccessMessage;
                        CurrentSchedule[daySchedule.Day].FailMesssage = daySchedule.FailMesssage;
                        CurrentSchedule[daySchedule.Day].OutOfTimeMessage = daySchedule.OutOfTimeMessage;
                    }
                    else
                    {
                        // if not then Add it to the db
                        db.DailyActiveData.Add(daySchedule);
                        CurrentSchedule.Add(daySchedule.Day, daySchedule);
                    }
                }
                var ScheduleDict = Schedule.ToDictionary(k => k.Day);
                // Go thorugh the existing days and remove any ones that were removed 
                foreach (var key in CurrentSchedule.Keys)
                {
                    if ( !ScheduleDict.ContainsKey(key) )
                    {
                        db.DailyActiveData.Remove(CurrentSchedule[key]);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<DayActiveGameView> GetSchedule()
        {
            var schedule = new List<DayActiveGameView>();
            try
            {
                using (var db = ApplicationDbContext.Create())
                {
                    var tmp = (from ag in db.DailyActiveData select ag).ToList();
                    schedule.AddRange(tmp.Where(d => GameManager.GameDict.ContainsKey(d.GameKey)).Select( d => new DayActiveGameView()
                    {
                        Day = d.Day,
                        GameKey = d.GameKey,
                        GameName = GameManager.GameDict[d.GameKey].Name,
                        SuccessMessage = d.SuccessMessage,
                        FailMesssage = d.FailMesssage,
                        OutOfTimeMessage = d.OutOfTimeMessage
                    }));
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
            }
            return schedule;

        }

        public static ActiveDailyData GetActiveDailyData(DateTime Date)
        {
            var GreekDate = DateHelper.GetGreekDate(Date, onlyDate: true);

            ActiveDailyData ActiveGameKey = null;
            try
            {
                using (var db = ApplicationDbContext.Create())
                {
                    var toDayStr = GreekDate.ToString("yyyy-MM-dd");

                    var Active = (from ag in db.DailyActiveData where ag.Day == toDayStr select ag).FirstOrDefault();
                    if (Active != null)
                    {
                        ActiveGameKey = new ActiveDailyData()
                        {
                            ActiveGameKey = Active.GameKey,
                            Messages = new Dictionary<string, string>()
                            {
                                { "success", Active.SuccessMessage ?? "" },
                                { "fail", Active.FailMesssage ?? ""},
                                { "outoftime", Active.OutOfTimeMessage ?? ""},
                                { "info", Active.InfoMessage }
                            }
                        };
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
            }
            return ActiveGameKey;
        }
    }
}