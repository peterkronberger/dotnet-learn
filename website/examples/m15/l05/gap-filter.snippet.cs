services.AddLogging(logging => logging
    .SetMinimumLevel(LogLevel.Information)
    .AddFilter("Notifier", LogLevel.___)
    .AddProvider(new ConsoleLoggerProvider()));
