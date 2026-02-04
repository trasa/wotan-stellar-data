using Microsoft.Extensions.Logging;
using WotanStellar.Command.Commands;

namespace WotanStellar.Command.Services;

public class GeneratePlanetsService : IRunnableService<GeneratePlanetsService>
{
    private readonly ILogger<GeneratePlanetsService> _logger;

    public GeneratePlanetsService(ILogger<GeneratePlanetsService> logger)
    {
        _logger = logger;
    }
    public Task RunAsync(CommandArguments args, ActionCommand<GeneratePlanetsService> actionCommand)
    {
        _logger.LogInformation("Generating planets {@Args} {@ActionCommand}", args, actionCommand);
        return Task.CompletedTask;
    }
}
