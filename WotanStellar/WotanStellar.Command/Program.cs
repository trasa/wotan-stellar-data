using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WotanStellar.Command.Commands;
using WotanStellar.Command.Services;
using WotanStellar.Data.Config;

namespace WotanStellar.Command;

class Program
{
    public static async Task<int> Main(string[] args)
    {
        var commandOptions = CommandOptions.Instance;
        var generatePlanetsCommand = new GeneratePlanetsCommand(commandOptions);

        var rootCommand = commandOptions.BuildRootCommand();
        rootCommand.Subcommands.Add(generatePlanetsCommand);
        return await rootCommand.Parse(args).InvokeAsync();
    }

    private static IHost BuildHost(CommandArguments args)
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
                config.AddJsonFile("appsettings.LocalUser.json", optional: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddScoped<GeneratePlanetsService>(); // data context
                services.AddWotanStellarData(options =>
                {
                    options.ConnectionString = context.Configuration.GetConnectionString("StellarData")!;
                    options.CommandTimeout = 60;
                    options.EnableSensitiveDataLogging = context.HostingEnvironment.IsDevelopment();
                });
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Debug);
                });
            })
            .Build();
    }

    public static async Task<int> RunHostAsync<TService>(CommandArguments args, ActionCommand<TService> command, CancellationToken cancellationToken = default) where TService : IRunnableService<TService>
    {
        using var host = BuildHost(args); // cancellationToken ?

        var svc = host.Services.GetRequiredService<TService>();
        await svc.RunAsync(args, command); // cancellationToken

        await host.StopAsync(cancellationToken);
        return 0;
    }
}
