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
    public char SpectralClass { get; set; } // OBAFGKM
    public double? SpectralSubClass { get; set; } // 0-10, 0 hotter, 10 cooler
    public string? LuminosityClass { get; set; } // V for main sequence
}

public class DegenerateSpectralType : ISpectralType
{
    public char SpectralClass => 'D'; // Degenerates (white dwarfs)
    public string SubType { get; set; } = string.Empty;
}
