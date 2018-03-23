using System;

namespace LaesoeLineApi
{
    public static class LaesoeTime
    {
        private static Lazy<TimeZoneInfo> _europeCopenhagen = new Lazy<TimeZoneInfo>(() => TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));

        public static TimeZoneInfo EuropeCopenhagen => _europeCopenhagen.Value;

        public static DateTime Now => TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, EuropeCopenhagen);
    }
}
