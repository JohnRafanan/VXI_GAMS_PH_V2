using System;

namespace VXI_GAMS_US.HELPER
{
    /// <summary>
    /// Get the datetime for the GMT +8
    /// </summary>
    public static class DateTimeHelper
    {
        public static DateTime GetDate => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
        public static DateTime GetDateNoTime
        {
            get
            {
                var datetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                    TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                return Convert.ToDateTime(datetime.ToString("MM/dd/yyyy"));
            }
        }

        public static DateTime ToDateTime(string dateTime)
        {
            DateTime.TryParse(dateTime, out var result);
            return result;
        }
        public static DateTime? ToDateTimeNullable(string dateTime)
        {
            DateTime? dt = null;
            if (!string.IsNullOrEmpty(dateTime))
                dt = Convert.ToDateTime(dateTime);
            return dt;
        }
    }
}
