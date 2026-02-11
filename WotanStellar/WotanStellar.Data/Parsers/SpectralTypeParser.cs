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
        var (luminosityClass, _)= _luminosityParser.Parse(spectralType);
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
}
