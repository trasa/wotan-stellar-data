using StackExchange.Redis;
using WotanStellar.Actors;
using WotanStellar.Data.Config;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional:false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.LocalUser.json", optional: true);
}
builder.Configuration.AddEnvironmentVariables();

// TODO settings:
// var mysettings = new MySettings();
// builder.Configuration.Bind("MySettings", mysettings);
// builder.Services.AddSingleton(mysettings);

// need http client?
//builder.Services.AddHttpClient();

builder.UseOrleans(static siloBuilder =>
{
    // re bind settings for orleans:
    // var mysettings = ...
    // siloBuilder.Configuration.Bind("");

    siloBuilder.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        // TODO look up from config
        logging.SetMinimumLevel(LogLevel.Debug);
        logging.AddSimpleConsole(options =>
        {
            options.UseUtcTimestamp = true;
            options.IncludeScopes = true;
            options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fffZ "; // UTC iso
        });
    });

    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddRedisGrainStorageAsDefault(options =>
    {
        options.ConfigurationOptions = ConfigurationOptions.Parse(siloBuilder.Configuration.GetConnectionString("GrainStorage") ??
                                                                  throw new InvalidOperationException("Missing Redis connection string"));
    });
});

// our (postgres) database -- not used for actor storage
builder.Services.AddWotanStellarData(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("StellarData")!;
    options.CommandTimeout = 60;
    options.EnableSensitiveDataLogging = builder.Environment.IsDevelopment();
});

//builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
