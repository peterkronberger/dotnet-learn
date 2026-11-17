// Composition root: the container puts the readers together
ServiceCollection services = new();
services.AddSingleton<IReportWriter, ConsoleReportWriter>();
services.AddSingleton<LogReaderFactory>();
using ServiceProvider provider = services.BuildServiceProvider();
LogReaderFactory readers = provider.GetRequiredService<LogReaderFactory>();
LogSourceReader webReader = readers.Create("web", logSources["web"]);
LogSourceReader vpnReader = readers.Create("vpn", logSources["vpn"]);
