using System;
using System.Globalization;

namespace Database
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Get only the Date part of DateTime.
        /// </summary>
        public static string AsDate(this DateTime time)
        {
            return time.ToString("d.M.yyyy", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Get only the Time part of DateTime.
        /// </summary>
        public static string AsTime(this DateTime time)
        {
            return time.ToString("HH:mm:ss", CultureInfo.CurrentCulture);
        }
    }
}
