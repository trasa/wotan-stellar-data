using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WotanStellar.Command.Commands;
using WotanStellar.Data;
using WotanStellar.Data.Generators;

namespace WotanStellar.Command.Services;

public class GeneratePlanetRingsService : IRunnableService<GeneratePlanetRingsService>
{
    private readonly ILogger<GeneratePlanetRingsService> _logger;
    private readonly StellarContext _stellarContext;
    private readonly IPlanetaryRingGenerator _ringGenerator;

    public GeneratePlanetRingsService(ILogger<GeneratePlanetRingsService> logger, StellarContext stellarContext, IPlanetaryRingGenerator ringGenerator)
    {
        _logger = logger;
        _stellarContext = stellarContext;
        _ringGenerator = ringGenerator;
    }

    public async Task RunAsync(CommandArguments args, ActionCommand<GeneratePlanetRingsService> actionCommand)
    {
        var planets = _stellarContext.Planets
            .Include(p => p.Moons)
            .Where(p => p.StarId != 1); // skip Sol

        foreach (var planet in planets)
        {
            var rs = _ringGenerator.Generate(planet);
            if (rs != null)
            {
                planet.RingSystem = rs;
            }
        }
        try
        {
            await _stellarContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save generated planets and moons");
        }
    }
}
