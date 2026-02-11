namespace WotanStellar.Data.Entities;

/// <summary>
/// see https://en.wikipedia.org/wiki/Stellar_classification
/// </summary>
public interface ISpectralType
{
    char SpectralClass { get; }
}

public class MainSequenceSpectralType : ISpectralType
{
    public char SpectralClass { get; set; }
    public double? SpectralSubClass { get; set; }
    public string? LuminosityClass { get; set; }
}

public class DegenerateSpectralType : ISpectralType
{
    public char SpectralClass => 'D'; // Degenerates (white dwarfs)
    public double? TemperatureClass { get; set; }
    public double? SurfaceTemperatureKelvin => 50400.0 / TemperatureClass;
}
