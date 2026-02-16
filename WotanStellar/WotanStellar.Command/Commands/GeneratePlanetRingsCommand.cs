using WotanStellar.Command.Services;

namespace WotanStellar.Command.Commands;

public class GeneratePlanetRingsCommand : ActionCommand<GeneratePlanetRingsService>
{
    public GeneratePlanetRingsCommand(CommandOptions options) : base(options, "generate-planet-rings", "generate rings for planets")
    {
        // TODO option -F --force
    }
}
