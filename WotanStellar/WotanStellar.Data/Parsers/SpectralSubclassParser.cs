namespace WotanStellar.Data.Parsers;

public interface ISpectralSubclassParser
{
    /// <summary>
    /// Parse subclass out of a spectral type fragment.
    /// The fragment must already have the spectral class letter(s) consumed.
    /// </summary>
    /// <remarks>
    /// Examples:
    ///     "A1 V"       -> "1 V"       -> (1.0, "V")
    ///     "A7 IV-V"    -> "7 IV-V"    -> (7.0, "IV-V")
    ///     "DA5.5+dM+dM -> "5.5+dM+dM" -> (5.5, "+dM+dM")
    ///     "DA" ->      -> ""          ->(null, "")
    /// </remarks>
    /// <param name="s"></param>
    /// <returns>parsed remainder string (what wasn't consumed), and parsed subclass value or null if not determined</returns>
    (double? value, string remainder) Parse(string s);
}
public class SpectralSubclassParser : ISpectralSubclassParser
{

    public (double? value, string remainder) Parse(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return (null, "");
        }

        // Extract numeric part from start of string, if any
        for(var i = 0; i < s.Length; i++)
        {
            if (!char.IsDigit(s[i]) && s[i] != '.')
            {
                // Found first non-numeric character, parse the numeric part
                var numericPart = s[..i];
                var remainder = s[i..].Trim();
                if (double.TryParse(numericPart, out var subClass))
                {
                    return (Math.Clamp(subClass, 0, 11), remainder);
                }
                return (null, remainder);
            }
        }

        // whole string is numeric
        if (double.TryParse(s, out var subClassAll))
        {
            return (Math.Clamp(subClassAll, 0, 11), "");
        }

        return (null, s);

/*
        // Extract numeric part after spectral class letter
            // E.g., "G8.5 V" -> 8.5, "M5" -> 5

            if (spectralType.Length < 2) {
                return 5.0; // Default to middle
            }
            var startIndex = 1;
            var endIndex = startIndex;

            // Find the end of the numeric part
            while (endIndex < spectralType.Length &&
                   (char.IsDigit(spectralType[endIndex]) || spectralType[endIndex] == '.'))
            {
                endIndex++;
            }

            if (endIndex > startIndex)
            {
                var numericPart = spectralType.Substring(startIndex, endIndex - startIndex);
                if (double.TryParse(numericPart, out double subClass))
                {
                    return Math.Clamp(subClass, 0, 9);
                }
            }
            return 5.0; // Default to middle
            */

    }
}
