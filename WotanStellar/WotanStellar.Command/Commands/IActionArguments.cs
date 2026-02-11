using WotanStellar.Command.Services;

namespace WotanStellar.Command.Commands;

public interface IActionArguments<TService> where TService : IRunnableService<TService>
{
}

public class NoActionArguments<TService> : IActionArguments<TService> where TService : IRunnableService<TService>
{
}
