using System;

public static class EnumHelper
{
    public static T Parse<T>(string value) where T : struct, Enum
    {
        return (T)Enum.Parse(typeof(T), value);
    }

    public static bool TryParse<T>(string value, out T result) where T : struct, Enum
    {
        return Enum.TryParse(value, out result);
    }
}
