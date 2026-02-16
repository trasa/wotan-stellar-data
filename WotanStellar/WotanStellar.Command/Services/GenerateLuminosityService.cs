using Microsoft.Extensions.Logging;
using WotanStellar.Command.Commands;
using WotanStellar.Data;
using WotanStellar.Data.Calculators;
using WotanStellar.Data.Parsers;

namespace WotanStellar.Command.Services;

public class GenerateLuminosityService : IRunnableService<GenerateLuminosityService>
{
    private readonly ILogger<GenerateLuminosityService> _logger;
    private readonly StellarContext _stellarContext;
    private readonly ISpectralTypeParser _spectralTypeParser;
    private readonly ILuminosityCalculator _luminosityCalculator;

    public GenerateLuminosityService(ILogger<GenerateLuminosityService> logger, StellarContext stellarContext, ISpectralTypeParser spectralTypeParser, ILuminosityCalculator luminosityCalculator)
    {
        _logger = logger;
        _stellarContext = stellarContext;
        _spectralTypeParser = spectralTypeParser;
        _luminosityCalculator = luminosityCalculator;
    }

    public async Task RunAsync(CommandArguments args, ActionCommand<GenerateLuminosityService> actionCommand)
    {
        foreach (var star in _stellarContext.Stars)
        {
            if (star.Name == "Sol")
            {
                _logger.LogInformation("Skipping Sol, it already has a known luminosity value");
                star.Luminosity = 1.0; // set Sol's luminosity to 1 L☉
                continue;
            }
            _logger.LogInformation("Processing star {Id}, {Name} {StellarType}", star.Id, star.Name, star.StarSpectralTypeSize);
            if (star.StarSpectralTypeSize == null)
            {
                _logger.LogWarning("Star {Id}, {Name} has no spectral type, skipping luminosity calculation", star.Id, star.Name);
            }
            else
            {
                var stellarType = _spectralTypeParser.Parse(star.StarSpectralTypeSize);
                var luminosity = _luminosityCalculator.CalculateLuminosity(stellarType);
                if (luminosity != null)
                {
                    star.Luminosity = luminosity.Value;
                    _logger.LogInformation("Calculated luminosity for star {Id}, {Name}: {Luminosity} L☉",
                        star.Id, star.Name, luminosity.Value);
                }
                else
                {
                    _logger.LogWarning("Failed to calculate luminosity for star {Id}, {Name} with spectral type {SpectralType}",
                        star.Id, star.Name, star.StarSpectralTypeSize);
                }
            }
        }
        try
        {
            await _stellarContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save luminosity values to database");
        }
    }
}
