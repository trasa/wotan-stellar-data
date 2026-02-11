using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using WotanStellar.Data.Calculators;
using WotanStellar.Data.Formatters;
using WotanStellar.Data.Generators;
using WotanStellar.Data.Parsers;
using WotanStellar.Data.Pathfinders;

namespace WotanStellar.Data.Config;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddWotanStellarData(this IServiceCollection services, Action<StellarDataOptions> configureOptions)
    {
        var options = new StellarDataOptions();
        configureOptions(options);

        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            throw new InvalidOperationException("ConnectionString must be provided in StellarDataOptions.");
        }

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(options.ConnectionString);
        dataSourceBuilder.UseNetTopologySuite();

        var dataSource = dataSourceBuilder.Build();
        services.AddSingleton(dataSource);

        services.AddDbContext<StellarContext>(dbOptions =>
        {
            dbOptions.UseNpgsql(dataSource, npgsqlOptions =>
            {
                npgsqlOptions.UseNetTopologySuite();
                if (options.CommandTimeout > 0)
                {
                    npgsqlOptions.CommandTimeout(options.CommandTimeout);
                }
            });

            if (options.EnableSensitiveDataLogging)
            {
                dbOptions.EnableSensitiveDataLogging();
            }
        });

        services
            .AddScoped<IStellarPathfinder, StellarPathfinder>() // scoped because they use the DbContext which is not threadsafe
            .AddScoped<IRoutePathfinder, AStarPathfinder>() // scoped for same reason
            .AddSingleton<IPlanetGenerator, PlanetGenerator>()
            .AddSingleton<IMoonGenerator, MoonGenerator>()
            .AddSingleton<ILuminosityCalculator, LuminosityCalculator>()
            .AddSingleton<RomanNumeralFormatter>()
            .AddSingleton<IFormatProvider>(sp => sp.GetRequiredService<RomanNumeralFormatter>())
            .AddSingleton<IDegenerateParser, DegenerateParser>()
            .AddSingleton<ILuminosityParser, LuminosityParser>()
            .AddSingleton<ISpectralTypeParser, SpectralTypeParser>()
            .AddSingleton<ISpectralSubclassParser, SpectralSubclassParser>();


        return services;

    }
}
