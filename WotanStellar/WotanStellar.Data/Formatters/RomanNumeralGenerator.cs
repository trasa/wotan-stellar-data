using System.Text;

namespace WotanStellar.Data.Formatters;


public class RomanNumeralFormatter : IFormatProvider, ICustomFormatter
{
    public static readonly RomanNumeralFormatter Instance = new();

    private static readonly (int, string)[] RomanNumerals = {
        (1000, "M"),
        (900, "CM"),
        (500, "D"),
        (400, "CD"),
        (100, "C"),
        (90, "XC"),
        (50, "L"),
        (40, "XL"),
        (10, "X"),
        (9, "IX"),
        (5, "V"),
        (4, "IV"),
        (1, "I")
    };

    public object? GetFormat(Type? formatType)
    {
        if (formatType == typeof(ICustomFormatter))
        {
            return this;
        }

        return null;

    }

    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        // If it's not the roman format, use default formatting
        if (format != "R" && format != "r")
        {
            return arg is IFormattable formattable
                ? formattable.ToString(format, formatProvider)
                : arg?.ToString() ?? string.Empty;
        }

        // Only handle integer types
        if (arg is not int and not long and not short and not byte)
        {
            return arg?.ToString() ?? string.Empty;
        }

        int number = Convert.ToInt32(arg);
        return ToRomanNumeral(number);
    }

    public string ToRomanNumeral(int number)
    {
        if (number is < 1 or > 3999)
            throw new ArgumentOutOfRangeException(nameof(number),
                "Roman numerals only support numbers between 1 and 3999");
        var result = new StringBuilder();
        foreach (var (value, numeral) in RomanNumerals)
        {
            while (number >= value)
            {
                result.Append(numeral);
                number -= value;
            }
        }

        return result.ToString();
    }
}
