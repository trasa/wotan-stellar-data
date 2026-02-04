using System.CommandLine;

namespace WotanStellar.Command;

public class CommandOptions
{
    public static readonly CommandOptions Instance = new();

    private Option<bool> ConfirmOption { get; } = new(name: "--yes", aliases: ["-y"])
    {
        Description = "Skip confirmation prompts and assume 'yes' for all questions."
    };

    public RootCommand BuildRootCommand()
    {
        return new RootCommand("WotanStellar Command Line Tool")
        {
            ConfirmOption
        };
    }

    public CommandArguments BuildArguments(ParseResult parseResult)
    {
        return new CommandArguments (
            Confirm: parseResult.GetValue(ConfirmOption)
        );
    }
}
