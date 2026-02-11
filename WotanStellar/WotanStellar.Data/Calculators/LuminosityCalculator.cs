using WotanStellar.Data.Entities;

namespace WotanStellar.Data.Calculators;
public interface ILuminosityCalculator
{
    public double? CalculateLuminosity(ISpectralType? spectralType);
}

public class LuminosityCalculator : ILuminosityCalculator
{

    // Luminosity classes (Roman numerals in spectral type)
    private static readonly Dictionary<string, double> LuminosityClassMultipliers = new()
    {
        { "0", 100000 },   // Hypergiants
        { "Ia", 10000 },   // Luminous supergiants
        { "Ib", 5000 },    // Less luminous supergiants
        { "Iab", 7500 },   // Intermediate supergiants
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

    public double? CalculateLuminosity(ISpectralType? spectralType)
    {
        return spectralType switch
        {
            MainSequenceSpectralType mainSequence => CalculateLuminosity(mainSequence),
            DegenerateSpectralType degenerate => CalculateLuminosity(degenerate),
            CompoundSpectralType compound => CalculateLuminosity(compound),
            _ => null
        };
    }

    private double? CalculateLuminosity(CompoundSpectralType spectralType)
    {
        // For simplicity, we can average the luminosities of the components
        var luminosities = spectralType
            .Components
            .Select(CalculateLuminosity)
            .Where(l => l.HasValue)
            .Select(l => l!.Value)
            .ToList();
        if (luminosities.Count == 0)
            return null;

        return luminosities.Average();
    }

    private double? CalculateLuminosity(MainSequenceSpectralType spectralType)
    {
        // Calculate base luminosity for main sequence
        if (!MainSequenceLuminosity.TryGetValue(spectralType.SpectralClass, out var value))
            return null;

        var (minLum, maxLum) = value;

        // Interpolate based on subclass (0 = hotter/brighter, 9 = cooler/dimmer)
        var subClass = spectralType.SpectralSubClass ?? 5.0; // Default to middle if no subclass
        var baseLuminosity = maxLum - (maxLum - minLum) * (subClass / 10.0);

        // Apply luminosity class multiplier
        var multiplier = 1.0;
        if (LuminosityClassMultipliers.TryGetValue(spectralType.LuminosityClass ?? "V", out var classMultiplier))
        {
            multiplier = classMultiplier;
        }

        return baseLuminosity * multiplier;
    }

    private double CalculateLuminosity(DegenerateSpectralType spectralType)
    {
        const int TEMP_CONSTANT = 50400;
        const double STEFAN_BOLTZMANN = 5.670374419e-8;
        const double SOLAR_RADIUS = 6.957e8;
        const double SOLAR_LUMINOSITY = 3.828e26; // watts
        const double WD_RADIUS = 0.01 * SOLAR_RADIUS; // meters

        var t_eff = TEMP_CONSTANT / (spectralType.TemperatureClass ?? 10.0);

        // Calculate luminosity using Stefan-Boltzmann law
        // L = 4π R² σ T⁴
        var luminosity = 4 * 3.14159265359 * Math.Pow(WD_RADIUS, 2) * STEFAN_BOLTZMANN * Math.Pow(t_eff, 4);
        return luminosity / SOLAR_LUMINOSITY; // Return in solar luminosities
    }
}
