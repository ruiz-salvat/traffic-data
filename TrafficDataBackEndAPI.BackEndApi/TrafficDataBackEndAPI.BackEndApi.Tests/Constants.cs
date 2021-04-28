using System;

namespace TrafficDataBackEndAPI.BackEndApi.Tests
{
    public class Constants
    {
        public const int ValidId = 1;
        public const int InvalidId = 999;
        public const int InvalidState = 99;
        public static DateTime StartDatetime = DateTime.Parse("2001-01-01 00:00:00");
        public static DateTime EndDatetime = DateTime.Parse("2100-12-31 23:59:59");
        public static DateTime MockDatetime = DateTime.Parse("2019-12-01 22:00:00");
        public const string StartDatetimeString = "2001-01-01 00:00:00";
        public const string EndDatetimeString = "2100-12-31 23:59:59";
        public const int NumberOfRetries = 10;
        public const int DelayOnRetry = 1000;
    }
}