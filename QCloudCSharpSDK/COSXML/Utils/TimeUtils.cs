using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Utils
{
    public sealed class TimeUtils
    {
        // utc start time
        public static readonly DateTime UTC_START_TIME = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
       
        public static long GetCurrentTime(TimeUnit timeUnit)
        {
            TimeSpan timeSpan = DateTime.UtcNow - UTC_START_TIME;
            long result = -1L;

            switch (timeUnit)
            {
                case TimeUnit.DAYS:
                    result = (long)timeSpan.TotalDays;
                    break;
                case TimeUnit.HOURS:
                    result = (long)timeSpan.TotalHours;
                    break;
                case TimeUnit.MINUTES:
                    result = (long)timeSpan.TotalMinutes;
                    break;
                case TimeUnit.SECONDS:
                    result = (long)timeSpan.TotalSeconds;
                    break;
                case TimeUnit.MILLISECONDS:
                    result = (long)timeSpan.TotalMilliseconds;
                    break;
            }

            return result;
        }

        public static string GetFormatTime(string format, long time, TimeUnit timeUnit)
        {
            DateTime end = DateTime.MinValue;
            DateTime start = UTC_START_TIME;

            switch (timeUnit)
            {
                case TimeUnit.DAYS:
                    end = start.AddDays(time);
                    break;
                case TimeUnit.HOURS:
                    end = start.AddHours(time);
                    break;
                case TimeUnit.MINUTES:
                    end = start.AddMinutes(time);
                    break;
                case TimeUnit.SECONDS:
                    end = start.AddSeconds(time);
                    break;
                case TimeUnit.MILLISECONDS:
                    end = start.AddMilliseconds(time);
                    break;
            }
            end = TimeZone.CurrentTimeZone.ToLocalTime(end);

            return end.ToString(format);
        }
    }

    public enum TimeUnit
    {
        MILLISECONDS = 0,

        SECONDS,

        MINUTES,

        HOURS,

        DAYS,
    }
}
