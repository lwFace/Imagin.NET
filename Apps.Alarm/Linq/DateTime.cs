using System;

namespace Alarm.Linq
{
    public static class DateTimeExtensions
    {
        public static string Relative(this DateTime input, string prefix)
        {
            const int second
                = 1;
            const int minute 
                = 60 * second;
            const int hour 
                = 60 * minute;
            const int day 
                = 24 * hour;
            const int month 
                = 30 * day;

            var span = default(TimeSpan);
            var delta = 0d;

            var now = DateTime.Now;
            var suffix = string.Empty;

            //It's in the future
            if (input > now)
            {
                span = new TimeSpan(input.Ticks - DateTime.Now.Ticks);
                delta = Math.Abs(span.TotalSeconds);
            }
            //It's in the past
            else if (input < now)
            {
                span = new TimeSpan(DateTime.Now.Ticks - input.Ticks);
                delta = Math.Abs(span.TotalSeconds);
                suffix = " ago";
            }
            //It's now
            else return "now";

            string result()
            {
                if (delta < 1 * minute)
                {
                    switch (span.Seconds)
                    {
                        case 0:
                        case 1:
                            return $"a second";
                        default:
                            return $"{span.Seconds} seconds";
                    }
                }

                if (delta < 2 * minute)
                    return $"a minute";

                if (delta < 45 * minute)
                    return $"{span.Minutes} minutes";

                if (delta < 120 * minute)
                    return $"an hour";

                if (delta < 24 * hour)
                    return $"{span.Hours} hours";

                if (delta < 48 * hour)
                {
                    if (input < now)
                    {
                        return "yesterday";
                    }
                    else if (input > now)
                        return "tomorrow";
                }

                if (delta < 30 * day)
                    return $"{span.Days} days";

                if (delta < 12 * month)
                {
                    var months = Convert.ToInt32(Math.Floor((double)span.Days / 30));

                    if (months <= 1)
                    {
                        return $"a month";
                    }
                    else return $"{months} months";
                }
                else
                {
                    var years = Convert.ToInt32(Math.Floor((double)span.Days / 365));

                    if (years <= 1)
                    {
                        return $"a year";
                    }
                    else return $"{years} years";
                }
            }
            return $"{prefix} {result()}{suffix}";
        }
    }
}