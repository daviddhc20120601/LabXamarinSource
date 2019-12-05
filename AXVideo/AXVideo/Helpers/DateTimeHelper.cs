using System;

namespace AXVideo.Helpers
{
    public static class DateTimeHelper
    {
        public static TimeSpan SubtractTimeSpan(this DateTime time)
        {
            if (time.Kind == DateTimeKind.Utc)
                time = time.ToLocalTime();
            TimeSpan span = (time - DateTime.Now).Duration();

            return span;
        }

        public static string GetTimeString(this DateTime startTime)
        {
            if (startTime.Kind == DateTimeKind.Utc)
                startTime = startTime.ToLocalTime();
            TimeSpan span = startTime.SubtractTimeSpan();
            if (DateTime.Compare(startTime, DateTime.Now) > 0)
            {
                if (span.TotalDays > 30)
                {
                    return $"{startTime.ToString("m", new System.Globalization.CultureInfo("zh-CN"))}";
                }
                else if (span.TotalDays > 21)
                {
                    return $"还有3周 {startTime.ToString("m", new System.Globalization.CultureInfo("zh-CN"))}·{startTime.ToString("HH:mm")}";
                }
                else if (span.TotalDays > 14)
                {
                    return $"还有2周 {startTime.ToString("m", new System.Globalization.CultureInfo("zh-CN"))}·{startTime.ToString("HH:mm")}";
                }
                else if (span.TotalDays > 7)
                {
                    return $"还有1周 {startTime.ToString("m", new System.Globalization.CultureInfo("zh-CN"))}·{startTime.ToString("HH:mm")}";
                }
                else if (span.TotalDays > 1)
                {
                    return $"还有{(int)Math.Floor(span.TotalDays)}天 {startTime.ToString("m", new System.Globalization.CultureInfo("zh-CN"))}·{startTime.ToString("HH:mm")}";
                }
                else if (startTime.Date.Equals(DateTime.Now.Date))
                {
                    return $"今天·{startTime.ToString("HH:mm")}";
                }
                else
                {
                    return $"还有{(int)Math.Floor(span.TotalHours)}小时 {startTime.ToString("m", new System.Globalization.CultureInfo("zh-CN"))}·{startTime.ToString("HH:mm")}";
                }
            }
            else
            {
                if (span.TotalDays > 30)
                {
                    return $"{startTime.ToString("m", new System.Globalization.CultureInfo("zh-CN"))}";
                }
                else if (span.TotalDays > 21)
                {
                    return "3周前";
                }
                else if (span.TotalDays > 14)
                {
                    return "2周前";
                }
                else if (span.TotalDays > 7)
                {
                    return "1周前";
                }
                else if (span.TotalDays > 1)
                {
                    return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
                }
                else if (span.TotalHours > 1)
                {
                    return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
                }
                else if (span.TotalMinutes > 1)
                {
                    return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
                }
                else if (span.TotalSeconds >= 1)
                {
                    return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
                }
                else
                {
                    return "刚刚";
                }
            }
        }
    }
}
