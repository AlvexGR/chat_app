using System;

namespace ChatApp.Utilities.Extensions
{
    public static class EnumExtension
    {
        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        public static T ToEnum<T>(this int value) where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
