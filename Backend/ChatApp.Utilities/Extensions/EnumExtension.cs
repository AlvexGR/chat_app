using System;

namespace ChatApp.Utilities.Extensions
{
    public static class EnumExtension
    {
        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }
    }
}
