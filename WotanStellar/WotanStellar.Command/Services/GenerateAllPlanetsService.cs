using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WotanStellar.Command.Commands;
using WotanStellar.Data;
using WotanStellar.Data.Generators;

namespace WotanStellar.Command.Services;

public class GenerateAllPlanetsService : IRunnableService<GenerateAllPlanetsService>
{
    private readonly ILogger<GenerateAllPlanetsService> _logger;
    private readonly StellarContext _stellarContext;
    private readonly IPlanetGenerator _planetGenerator;

    public GenerateAllPlanetsService(ILogger<GenerateAllPlanetsService> logger, StellarContext stellarContext, IPlanetGenerator planetGenerator)
    {
        _logger = logger;
        _stellarContext = stellarContext;
        _planetGenerator = planetGenerator;
    }

    public async Task RunAsync(CommandArguments args, ActionCommand<GenerateAllPlanetsService> actionCommand)
    {
        var starSystems = _stellarContext.StarSystems
            .Include(ss => ss.Stars)
            .ThenInclude(s => s.Planets);

        foreach (var system in starSystems)
        {
            if (system.Stars.Count == 0)
            {
                _logger.LogWarning("Star System {Id} {Name} has no stars!", system.Id, system.Name);
            }
            else
            {
                _logger.LogInformation("Generating planets for system {Id}, {Name}", system.Id, system.Name);
                _planetGenerator.GeneratePlanets(system);
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
