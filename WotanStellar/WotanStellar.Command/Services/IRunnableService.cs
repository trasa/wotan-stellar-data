using WotanStellar.Command.Commands;

namespace WotanStellar.Command.Services;

public interface IRunnableService<TService> where TService: IRunnableService<TService>
{
    Task RunAsync(CommandArguments args, ActionCommand<TService> actionArgs);

}
