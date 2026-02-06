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
        _logger.LogInformation("Generating planets {@Args} {@ActionCommand}", args, actionCommand);
        var command = (GeneratePlanetsCommand)actionCommand;
        var id = actionCommand.TypedArguments<GeneratePlanetsArguments>().StarSystemId;

        var starSystem = await _stellarContext.StarSystems.Where(ss => ss.Id == id).FirstOrDefaultAsync();
        if (starSystem == null)
        {
            _logger.LogWarning("Star System {SystemId} not found!", id);
            return;
        }
        _logger.LogInformation("Generating planets for system {Id}, {Name}", id, starSystem.SystemName);
        _planetGenerator.GeneratePlanets(starSystem);
    }
}
