using WotanStellar.Data.Formatters;

namespace WotanStellar.Data.Parsers;

public interface ILuminosityParser
{
    (string? luminosity, string remainder) Parse(string s);
}

public class LuminosityParser : ILuminosityParser
{
    public (string? luminosity, string remainder) Parse(string s)
    {
        var parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var start = parts.FirstOrDefault();
        if (string.IsNullOrEmpty(start))
        {
            return (null, s);
        }

        // some exceptions to the roman numeral convention:
        if (start.Equals("0", StringComparison.OrdinalIgnoreCase))
        {
            return ("0", string.Join(' ', parts.Skip(1)));
        }
        if (start.Equals("Ia+", StringComparison.OrdinalIgnoreCase))
        {
            return ("Ia+", string.Join(' ', parts.Skip(1)));
        }
        if (start.Equals("Ia", StringComparison.OrdinalIgnoreCase))
        {
            return ("Ia", string.Join(' ', parts.Skip(1)));
        }
        if (start.Equals("Iab", StringComparison.OrdinalIgnoreCase))
        {
            return ("Iab", string.Join(' ', parts.Skip(1)));
        }
        if (start.Equals("Ib", StringComparison.OrdinalIgnoreCase))
        {
            return ("Ib", string.Join(' ', parts.Skip(1)));
        }
        // otherwise try and parse roman numeral...
        // can have marginal types: "IV-V"
        var lastPart = start.TakeWhile(c => RomanNumeralFormatter.IsRomanChar(c) || c == '-').Count();
        if (lastPart == 0)
        {
            return (null, s);
        }

        var luminosity = start[..lastPart];
        var remainder = start[lastPart..].TrimStart() + string.Join(' ', parts.Skip(1));
        return (luminosity, remainder);
/*
        // Check if it matches a known luminosity class
        // return the key not the value
        var key = LuminosityClassMultipliers.Keys.FirstOrDefault(k => lastPart.Equals(k, StringComparison.OrdinalIgnoreCase));
        return key ?? "V"; // default to main sequence
        */
    }
}
