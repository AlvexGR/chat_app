using System;

namespace ChatApp.Utilities.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime Get()
        {
            return DateTime.UtcNow;
        }
    }
}
