using System;

namespace Custom.Utils
{
    public static class EnumUtils 
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}