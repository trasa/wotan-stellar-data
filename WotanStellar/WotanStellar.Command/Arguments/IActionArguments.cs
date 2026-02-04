using WotanStellar.Command.Services;

namespace WotanStellar.Command.Arguments;

public interface IActionArguments<TService> where TService : IRunnableService<TService>
{
}

public class NoActionArguments<TService> : IActionArguments<TService> where TService : IRunnableService<TService>
{
}
