using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Utilities
{
    public static class DateTimeUtil
    {
        public static DateTime GetUtcToday()
        {
            var utcNow = DateTime.UtcNow;
            DateTime today = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day);

            return today;
        }

        public static DateTime GetClientDay(string clientDay)
        {
            if (string.IsNullOrEmpty(clientDay))
                clientDay = DateTime.Now.Year + "," + DateTime.Now.Month + "," + DateTime.Now.Day;

            var dateStr = clientDay.Split(',');
            int year = Int32.Parse(dateStr[0]);
            int month = Int32.Parse(dateStr[1]);
            int day = Int32.Parse(dateStr[2]);
            DateTime clientToday = new DateTime(year, month, day);

            return clientToday;
        }

        public static int GetAge(DateTime date)
        {
            return (int)(DateTime.Now.Subtract(date).TotalDays / 365.25);
        }

        public static DateTime GetTimeFromClientStr(string clientDateStr)
        {
            try
            {
                var clientDateStrSplit = clientDateStr.Split('T');
                var dateStrSplit = clientDateStrSplit[0].Split('-');
                var timeStrSplit = clientDateStrSplit[1].Split(':');

                var year = Int32.Parse(dateStrSplit[0]);
                var month = Int32.Parse(dateStrSplit[1]);
                var day = Int32.Parse(dateStrSplit[2]);

                var hour = Int32.Parse(timeStrSplit[0]);
                var min = Int32.Parse(timeStrSplit[1]);
                var second = Int32.Parse(timeStrSplit[2].Split('.')[0]);

                var date = new DateTime(year, month, day, hour, min, second).ToUniversalTime();

                return date;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }

        }
    }
}