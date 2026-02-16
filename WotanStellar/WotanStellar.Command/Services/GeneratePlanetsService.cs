using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WotanStellar.Command.Commands;
using WotanStellar.Data;
using WotanStellar.Data.Generators;

namespace WotanStellar.Command.Services;

public class GeneratePlanetsService : IRunnableService<GeneratePlanetsService>
{
    private readonly ILogger<GeneratePlanetsService> _logger;
    private readonly StellarContext _stellarContext;
    private readonly IPlanetGenerator _planetGenerator;

    public GeneratePlanetsService(ILogger<GeneratePlanetsService> logger, StellarContext stellarContext, IPlanetGenerator planetGenerator)
    {
        _logger = logger;
        _stellarContext = stellarContext;
        _planetGenerator = planetGenerator;
    }

    public async Task RunAsync(CommandArguments args, ActionCommand<GeneratePlanetsService> actionCommand)
    {
        var id = actionCommand.TypedArguments<GeneratePlanetsArguments>().StarSystemId;

        var starSystem = await _stellarContext.StarSystems
            .Include(ss => ss.Stars)
            .Where(ss => ss.Id == id)
            .FirstOrDefaultAsync();

        if (starSystem == null)
        {
            _logger.LogWarning("Star System {SystemId} not found!", id);
            return;
        }

        if (starSystem.Stars.Count == 0)
        {
            _logger.LogWarning("Star System {SystemId} has no stars, skipping planet generation!", id);
            return;
        }
        _logger.LogInformation("Generating planets for system {Id}, {Name}", id, starSystem.Name);
        _planetGenerator.GeneratePlanets(starSystem);

        try
        {
            await _stellarContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save generated planets for system {Id}, {Name}", id, starSystem.Name);
        }
    }
}
