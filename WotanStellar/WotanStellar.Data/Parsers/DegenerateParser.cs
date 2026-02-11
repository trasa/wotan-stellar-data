using WotanStellar.Data.Entities;

namespace WotanStellar.Data.Parsers;

public interface IDegenerateParser
{
    DegenerateSpectralType Parse(string input);
}
public class DegenerateParser : IDegenerateParser
{
    public DegenerateSpectralType Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || !input.StartsWith('D'))
        {
            throw new ArgumentException("Input must start with 'D' for degenerate spectral types.");
        }

        var surfaceTemp = "";
        var hasDigits = false;
        foreach (var t in input)
        {
            if (char.IsDigit(t) || t == '.')
            {
                surfaceTemp += t;
                hasDigits = true;
            }
            else
            {
                if (hasDigits)
                {
                    // we're done
                    break;
                }
            }
        }

        if (double.TryParse(surfaceTemp, out var starTemp) && starTemp > 0)
        {
            return new DegenerateSpectralType
            {
                TemperatureClass = starTemp,
            };
        }

        return new DegenerateSpectralType();
    }
}
