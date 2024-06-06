using System;
using System.Text.RegularExpressions;

namespace VXI_GAMS_US.HELPER
{
    public class DataHelper
    {
        public static DateTime? ToDateTime(string dateTime)
        {
            DateTime? dt = null;
            if (!string.IsNullOrEmpty(dateTime))
                dt = Convert.ToDateTime(dateTime);
            return dt;
        }
        public static string AlphaSpaceOnly(string value)
        {
            value = Regex.Replace(value, @"[^a-zA-Z\s]", "").Trim();
            value = Regex.Replace(value.Trim(), @"[ ]{2,}", " ").ToUpper().Trim();
            return value;
        }
    }
}
