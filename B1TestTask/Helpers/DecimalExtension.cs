using System.Globalization;

namespace B1TestTask.Helpers;

public static class DecimalExtension
{
    public static decimal Normalize(this decimal value)
    {
        return value/1.000000000000000000000000000000000m;
    }
    public static string NormalizeAndReturnString(this decimal value)
    {
        value = value.Normalize();
        return value.ToString(CultureInfo.InvariantCulture);
    }
}
