using WotanStellar.Command.Services;

namespace WotanStellar.Command.Commands;

public class GeneratePlanetsCommand(CommandOptions options) : ActionCommand<GeneratePlanetsService>(options, "generate-planets", "generate planets for a star system")
{
}
