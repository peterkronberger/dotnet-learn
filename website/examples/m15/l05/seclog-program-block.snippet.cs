// Logging: the minimum level decides which messages are written, the provider where
services.AddLogging(logging => logging
    .SetMinimumLevel(LogLevel.Information)
    .AddProvider(new ConsoleLoggerProvider()));
services.AddSingleton(detector);
services.AddTransient<IncidentMonitor>();
services.AddSingleton<LineValidator>();
// ...
// Reject invalid lines: the validator logs every line it rejects
LineValidator validator = provider.GetRequiredService<LineValidator>();
validator.TryParse(sampleLog[1], 2);
validator.TryParse(brokenLines[0], 3);
validator.TryParse(brokenLines[1], 4);
// ...
IncidentMonitor notifier = provider.GetRequiredService<IncidentMonitor>();
