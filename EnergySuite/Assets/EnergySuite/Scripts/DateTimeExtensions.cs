using System;

namespace EnergySuite
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public static double ConvertToUnixTimestamp(this DateTime date)
        {
            var diff = date - UnixEpoch;
            return Math.Floor(diff.TotalSeconds);
        }

        public static DateTime ConvertFromUnixTimestamp(this DateTime dateTime, double timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp);
        }
    }
}
