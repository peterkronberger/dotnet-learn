// Composition root: the container puts the readers together
ServiceCollection services = new();
services.AddSingleton<IReportWriter, ConsoleReportWriter>();
services.AddSingleton<LogReaderFactory>();
// Lifetimes: shared state is a singleton, one run is scoped, a helper is transient
services.AddSingleton(TimeProvider.System);
services.AddSingleton<FailureCounter>();
services.AddScoped<AnalysisRun>();
services.AddTransient<FailureMatcher>();
using ServiceProvider provider = services.BuildServiceProvider();
LogReaderFactory readers = provider.GetRequiredService<LogReaderFactory>();
// One analysis run of both sources inside one scope
using (IServiceScope analysisScope = provider.CreateScope())
{
    AnalysisRun analysis = analysisScope.ServiceProvider.GetRequiredService<AnalysisRun>();
    analysis.Analyze(logSources["web"]);
    analysis.Analyze(logSources["vpn"]);
}
LogSourceReader webReader = readers.Create("web", logSources["web"]);
LogSourceReader vpnReader = readers.Create("vpn", logSources["vpn"]);
