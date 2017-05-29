using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Helpers
{
    public class DateHelper
    {
        public static DateTime GetLocalizedDateTime(DateTime Date, string Localization, bool onlyDate = true)
        {
            DateTime res;
            // if empty then use servers local date
            if (string.IsNullOrEmpty(Localization))
            {
                res = DateTime.Now;
            }
            else
            {
                var info = TimeZoneInfo.FindSystemTimeZoneById(Localization);

                DateTimeOffset dtOffset = new DateTimeOffset(Date);
                res = TimeZoneInfo.ConvertTime(dtOffset, info).DateTime;
            }

            if (onlyDate)
            {
                res = res.Date;
            }

            return res;
        }

        public static DateTime GetGreekDate(DateTime Date, bool onlyDate = true)
        {
            return GetLocalizedDateTime(Date, "GTB Standard Time", onlyDate);
        }
    }
}