using WotanStellar.Command.Services;

namespace WotanStellar.Command.Commands;

public class GenerateAllPlanetsCommand : ActionCommand<GenerateAllPlanetsService>
{
    public GenerateAllPlanetsCommand(CommandOptions options) : base(options, "generate-all-planets", "generate planets for all star systems")
    {
        // TODO option -F --force for if they already have planets, redo them
    }

}
