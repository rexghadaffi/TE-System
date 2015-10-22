using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class DateExtension
    {
        public static string DayToDate(DayOfWeek dayofweek, int currentWeek)
        {
            return FirstDateOfWeek(currentWeek).DayOfWeekToDate(dayofweek);
        }
        public static string Sunday(int currentWeek)
        {
            return FirstDateOfWeek(currentWeek).SundayToDate();
        }

        public static int SetWeek { get; set; }
        public static int CurrentWeek
        {
            get
            {
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;

                return cal.GetWeekOfYear(DateTime.Now, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            }
        }
        public static bool IsEntryModificationLocked 
        {
            get 
            {
                DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                if (currentDay > DayOfWeek.Friday || currentDay == DayOfWeek.Sunday)
                {
                    return true;
                }
                else if (currentDay == DayOfWeek.Friday && DateTime.Now.TimeOfDay > new TimeSpan(13, 0, 0))
                {
                    return true;
                }
                return false;
            }
        }       

        private static string DayOfWeekToDate(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = startOfWeek - DateTime.Now.DayOfWeek;
            return dt.AddDays(diff).Date.ToString("MMM d");
        }

        private static string SundayToDate(this DateTime dt)
        {
            int diff = DayOfWeek.Saturday - DateTime.Now.DayOfWeek;
            return dt.AddDays(diff).AddDays(1).Date.ToString("MMM d");
        }
        private static DateTime FirstDateOfWeek(int currentWeek)
        {
            DateTime jan1 = new DateTime(DateTime.Now.Year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday,
                                              CalendarWeekRule.FirstFourDayWeek,
                                              DayOfWeek.Monday);

            var weekNum = currentWeek;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public static int GetWeekNumber(string date)
        {
            try
            {
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;
                return cal.GetWeekOfYear(Convert.ToDateTime(date),
                                                   dfi.CalendarWeekRule,
                                                   dfi.FirstDayOfWeek);
            }
            catch { return 0; }
        }
    }
}
