using System.CommandLine;
using WotanStellar.Command.Services;

namespace WotanStellar.Command.Commands;

public class GeneratePlanetsCommand : ActionCommand<GeneratePlanetsService>
{
    public GeneratePlanetsCommand(CommandOptions options) : base(options, "generate-planets", "generate planets for a star system")
    {
        Options.Add(StarSystemId);
    }

    public Option<int> StarSystemId { get; } = new(name:"--star-system-id", aliases: ["-s"])
    {
        Description = "The ID of the star system to generate planets for.",
        Required = true
    };

    protected override void PopulateActionArguments(ParseResult parseResult, CommandArguments args)
    {
        base.PopulateActionArguments(parseResult, args);
        ActionArguments = new GeneratePlanetsArguments
        {
            StarSystemId = parseResult.GetRequiredValue(StarSystemId)
        };
    }
}

public class GeneratePlanetsArguments : IActionArguments<GeneratePlanetsService>
{
    public int StarSystemId { get; set; }
}
