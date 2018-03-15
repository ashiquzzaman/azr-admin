using System;
using System.Globalization;
using System.Linq;

namespace AzR.Utilities.Exentions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek)
        {
            var diff = dateTime.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dateTime.AddDays(-1 * diff).Date;
        }

        public static string ToJulian(this DateTime dateTime)
        {
            var result = string.Format("{0}{1}", dateTime.Year, dateTime.DayOfYear);
            return result;
        }
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            var firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
        public static string ToFriendlyDateTime(this long value)
        {

            var dateTime = DateTime.ParseExact(value.ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            var localDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).ToLocalTime();
            var span = DateTime.UtcNow - localDateTime;

            if (span > TimeSpan.FromHours(24))
            {
                return dateTime.ToString("MMM dd");
            }

            if (span > TimeSpan.FromMinutes(60))
            {
                return string.Format("{0}h", span.Hours);
            }

            return span > TimeSpan.FromSeconds(60) ? string.Format("{0}m ago", span.Minutes) : "Just now";
        }
        public static DateTime ToDateTime(this string value)
        {
            var time = new DateTime();
            var matchingCulture =
                CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .FirstOrDefault(ci => DateTime.TryParse(value, ci, DateTimeStyles.None, out time));
            return time;
        }
        public static DateTime LongToDateTime(this long value)
        {
            var span = DateTime.ParseExact(value.ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            return span;
        }

        public static string LongDateTimeToString(this long value)
        {
            var span =
                DateTime.ParseExact(value.ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            var returnValue = span.ToString("dd-MM-yyyy");
            return returnValue;
        }
        public static long ToLong(this DateTime dateTime)
        {
            var date = dateTime.ToString("yyyyMMddHHmmss");
            var span = Convert.ToInt64(date);
            return span;
        }
    }
}
