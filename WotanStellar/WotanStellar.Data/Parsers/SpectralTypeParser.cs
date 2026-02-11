using Microsoft.Extensions.Logging;
using WotanStellar.Data.Entities;

namespace WotanStellar.Data.Parsers;

public interface ISpectralTypeParser
{
    public ISpectralType? Parse(string spectralType);
}

public class SpectralTypeParser : ISpectralTypeParser
{
    private readonly ILogger<SpectralTypeParser> _logger;
    private readonly ISpectralSubclassParser _subclassParser;
    private readonly ILuminosityParser _luminosityParser;
    private readonly IDegenerateParser _degenerateParser;

    public SpectralTypeParser(ILogger<SpectralTypeParser> logger,
        ISpectralSubclassParser subclassParser,
        ILuminosityParser luminosityParser,
        IDegenerateParser degenerateParser)
    {
        _logger = logger;
        _subclassParser = subclassParser;
        _luminosityParser = luminosityParser;
        _degenerateParser = degenerateParser;
    }

    // Luminosity classes (Roman numerals in spectral type)
    private static readonly Dictionary<string, double> LuminosityClassMultipliers = new()
    {
        { "0", 100000 },   // Hypergiants
        { "Ia", 10000 },   // Luminous supergiants
        { "Ib", 5000 },    // Less luminous supergiants
        { "II", 1000 },    // Bright giants
        { "III", 100 },    // Normal giants
        { "IV", 10 },      // Subgiants
        { "V", 1 },        // Main sequence (dwarfs)
        { "VI", 0.1 },     // Subdwarfs
        { "VII", 0.01 }    // White dwarfs
    };

    // Main sequence absolute luminosity by spectral class (in solar units)
    // These are approximate values for main sequence (class V) stars
    private static readonly Dictionary<char, (double min, double max)> MainSequenceLuminosity = new()
    {
        { 'O', (30000, 1000000) },  // O0-O9
        { 'B', (25, 30000) },       // B0-B9
        { 'A', (5, 25) },           // A0-A9
        { 'F', (1.5, 5) },          // F0-F9
        { 'G', (0.6, 1.5) },        // G0-G9
        { 'K', (0.08, 0.6) },       // K0-K9
        { 'M', (0.0001, 0.08) },    // M0-M9
        { 'L', (0.00001, 0.0001) }, // L0-L9 (brown dwarfs)
        { 'T', (0.000001, 0.00001) }, // T0-T9 (brown dwarfs)
        { 'Y', (0.0000001, 0.000001) }  // Y dwarfs
    };

    // White dwarf luminosities (much dimmer)
    private static readonly Dictionary<char, double> WhiteDwarfLuminosity = new()
    {
        { 'D', 0.0001 },  // DA, DB, DC, DO, DQ, DZ, etc.
    };

    public ISpectralType? Parse(string spectralType)
    {
        spectralType = spectralType.Trim().ToUpper();
        if (spectralType.StartsWith('D'))
        {
            return _degenerateParser.Parse(spectralType);
        }

        if (spectralType.Contains('/'))
        {
            return ParseCompound(spectralType.Split('/'));
        }

        // Parse main spectral class (O, B, A, F, G, K, M, L, T, Y)
        if (spectralType.Length == 0)
        {
            return null; // dunno
        }

        var spectralClass = spectralType[0];
        spectralType = spectralType[1..].Trim(); // Remove the spectral class letter for further parsing
        (var subClass, spectralType) = _subclassParser.Parse(spectralType);
        (var luminosityClass, spectralType) = _luminosityParser.Parse(spectralType); // TODO remainder?
        return new MainSequenceSpectralType
        {
            SpectralClass = spectralClass,
            SpectralSubClass = subClass,
            LuminosityClass = luminosityClass,
        };
    }

    private ISpectralType? ParseCompound(string[] parts)
    {
        var components = parts.Select(Parse).OfType<ISpectralType>().ToList();

        if (components.Count == 0)
        {
            return null;
        }
        return components.Count == 1 ? components[0] : new CompoundSpectralType { Components = components };
    }

    private double? EstimateLuminosity(string spectralType)
    {
        spectralType = spectralType.Trim().ToUpper();

        // Handle white dwarfs (D prefix)
        if (spectralType.StartsWith('D'))
        {
            var wd = ParseWhiteDwarf(spectralType);
            // TODO
            return null;
        }

        // Parse main spectral class (O, B, A, F, G, K, M, L, T, Y)
        if (spectralType.Length == 0)
            return null;

        var spectralClass = spectralType[0];

        if (!MainSequenceLuminosity.TryGetValue(spectralClass, out var value))
            return null;

        // Parse subclass (0-9, may have decimal like 8.5)
        (var subClass, spectralType) = _subclassParser.Parse(spectralType);

        // Parse luminosity class (V, IV, III, etc.)
        var luminosityClass = _luminosityParser.Parse(spectralType);

        // Calculate base luminosity for main sequence
        var (minLum, maxLum) = value;

        // Interpolate based on subclass (0 = hotter/brighter, 9 = cooler/dimmer)
        var baseLuminosity = maxLum - (maxLum - minLum) * (subClass / 10.0);

        // Apply luminosity class multiplier
        // var multiplier = 1.0;
        // if (LuminosityClassMultipliers.TryGetValue(luminosityClass, out var classMultiplier))
        // {
        //     multiplier = classMultiplier;
        // }
        //
        // return baseLuminosity * multiplier;
        throw new NotImplementedException();
    }

    private ISpectralType ParseWhiteDwarf(string spectralType)
    {
        // White dwarfs: DA, DB, DC, DO, DQ, DZ followed by temperature class
        // Temperature affects luminosity but they're all quite dim
        return new DegenerateSpectralType();
/*
        // Extract temperature class if present (0-9)
        var tempClass = 5.0;

        for (var i = 1; i < spectralType.Length; i++)
        {
            if (char.IsDigit(spectralType[i]))
            {
                tempClass = spectralType[i] - '0';
                break;
            }
        }

        // White dwarfs: hotter (lower number) = more luminous
        // Temperature class 0 = ~0.001 solar, class 9 = ~0.00001 solar
        var baseLuminosity = 0.001;
        var luminosity = baseLuminosity * Math.Pow(0.1, tempClass / 9.0);

        return luminosity;
        */
    }
}
