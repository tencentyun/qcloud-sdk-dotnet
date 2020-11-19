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
                case TimeUnit.Days:
                    result = (long)timeSpan.TotalDays;
                    break;
                case TimeUnit.Hours:
                    result = (long)timeSpan.TotalHours;
                    break;
                case TimeUnit.Minutes:
                    result = (long)timeSpan.TotalMinutes;
                    break;
                case TimeUnit.Seconds:
                    result = (long)timeSpan.TotalSeconds;
                    break;
                case TimeUnit.Milliseconds:
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
                case TimeUnit.Days:
                    end = start.AddDays(time);
                    break;
                case TimeUnit.Hours:
                    end = start.AddHours(time);
                    break;
                case TimeUnit.Minutes:
                    end = start.AddMinutes(time);
                    break;
                case TimeUnit.Seconds:
                    end = start.AddSeconds(time);
                    break;
                case TimeUnit.Milliseconds:
                    end = start.AddMilliseconds(time);
                    break;
            }

            end = TimeZone.CurrentTimeZone.ToLocalTime(end);

            return end.ToString(format);
        }
    }

    public enum TimeUnit
    {
        Milliseconds = 0,

        Seconds,

        Minutes,

        Hours,

        Days,
    }
}
