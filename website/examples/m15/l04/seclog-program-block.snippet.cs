// Composition root: the container puts the readers together
ServiceCollection services = new();
services.AddSingleton<IReportWriter, ConsoleReportWriter>();
services.AddSingleton<LogReaderFactory>();
services.AddSingleton(TimeProvider.System);
services.AddSingleton<FailureCounter>();
services.AddScoped<AnalysisRun>();
services.AddTransient<FailureMatcher>();
// Validation finds lifetime mistakes when the provider is built
ServiceProviderOptions providerOptions = new()
{
    ValidateScopes = true,
    ValidateOnBuild = true,
};
using ServiceProvider provider = services.BuildServiceProvider(providerOptions);
