// Create the generic host builder: configuration, logging and the container in one
HostApplicationBuilder builder = Host.CreateApplicationBuilder(
    new HostApplicationBuilderSettings
    {
        Args = args,
        ContentRootPath = AppContext.BaseDirectory,
    });
// Read the thresholds of the brute-force rule: the environment beats appsettings.json
IConfiguration configuration = builder.Configuration;
// ...
// Logging: the minimum level decides which messages are written, the provider where
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddFilter("Microsoft.Hosting", LogLevel.Warning);
builder.Logging.AddProvider(new ConsoleLoggerProvider());
// ...
// Options: bind the section to the options class and check its attributes
services.AddOptions<DetectionOptions>()
    .Bind(configuration.GetSection(DetectionOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
// Validation finds lifetime mistakes when the host builds the provider
builder.ConfigureContainer(new DefaultServiceProviderFactory(
    new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true }));
// The analysis runs in a background service that stops the host when it is done
services.AddHostedService(provider => new AnalysisService(
    () => RunAnalysisAsync(provider),
    provider.GetRequiredService<IHostApplicationLifetime>()));
using IHost host = builder.Build();
await host.RunAsync();
