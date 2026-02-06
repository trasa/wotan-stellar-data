using System.CommandLine;
using WotanStellar.Command.Arguments;
using WotanStellar.Command.Services;

namespace WotanStellar.Command.Commands;

public abstract class ActionCommand<TService> : System.CommandLine.Command where TService : IRunnableService<TService>
{
    public IActionArguments<TService> ActionArguments { get; set; } = new NoActionArguments<TService>();

    protected virtual void PopulateActionArguments(ParseResult parseResult, CommandArguments args)
    {
        // does nothing, ActionArguments is already NoActionArguments
    }

    public TArgs TypedArguments<TArgs>() where TArgs : class, IActionArguments<TService>
    {
        return ActionArguments as TArgs ?? throw new InvalidOperationException($"Invalid action arguments type. Expected {typeof(TArgs).Name}, but got {ActionArguments.GetType().Name}");
    }

    protected virtual bool Confirm(CommandArguments args) => true;

    protected ActionCommand(CommandOptions options, string name, string? description = null) : base(name, description)
    {
        SetAction((parseResult, cancellationToken) =>
        {
            var args = options.BuildArguments(parseResult);
            PopulateActionArguments(parseResult, args);
            return Confirm(args) ? Program.RunHostAsync(args, this, cancellationToken) : Task.CompletedTask;
        });
    }
}
