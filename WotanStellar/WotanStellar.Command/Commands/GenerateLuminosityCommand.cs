using WotanStellar.Command.Services;

namespace WotanStellar.Command.Commands;

public class GenerateLuminosityCommand  : ActionCommand<GenerateLuminosityService>
{
    public GenerateLuminosityCommand(CommandOptions options) : base(options, "generate-luminosity", "generate luminosity values for stars based on spectral type")
    {
    }
}
