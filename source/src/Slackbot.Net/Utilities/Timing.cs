using System;
using System.Collections.Generic;
using Cronos;

namespace Slackbot.Net.Utilities
{
    public class Timing
    {
        public TimeZoneInfo TimeZoneInfo;

        private TimeZoneInfo GetTimeZoneInfo(string timeZoneId)
        {
            if (!string.IsNullOrEmpty(timeZoneId))
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }

            if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Europe/Oslo");
            }
            return TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        }

        public DateTimeOffset RelativeNow(DateTimeOffset? nowutc = null)
        {
            return TimeZoneInfo.ConvertTime(nowutc ?? DateTimeOffset.UtcNow, TimeZoneInfo);
        }

        public bool IsToday(DateTime date)
        {
            var nowInOslo = RelativeNow();
            return nowInOslo.Month == date.Month && nowInOslo.Day == date.Day;
        }

        public int CalculateAge(DateTime birthDate)
        {
            var nowInOsloTime = RelativeNow();
            var age = nowInOsloTime.Year - birthDate.Year;

            if (nowInOsloTime.Month < birthDate.Month || (nowInOsloTime.Month == birthDate.Month && nowInOsloTime.Day < birthDate.Day))
                age--;

            return age;
        }

        public DateTimeOffset? GetNextOccurenceInRelativeTime(string cron)
        {
            var expression = CronExpression.Parse(cron, CronFormat.IncludeSeconds);
            return expression.GetNextOccurrence(DateTimeOffset.UtcNow, TimeZoneInfo);
        }

        public static IEnumerable<DateTime> GetNextOccurences(string cron, int noOfMonths = 0)
        {
            var expression = CronExpression.Parse(cron, CronFormat.IncludeSeconds);
            var fromUtc = DateTime.UtcNow;
            var toUtc = fromUtc.AddMonths(noOfMonths != 0 ? noOfMonths : 6);
            var nexts = expression.GetOccurrences(fromUtc,toUtc);
            return nexts;
        }

        public void SetTimeZone(string timeZoneId)
        {
            TimeZoneInfo = GetTimeZoneInfo(timeZoneId);
        }
    }
}