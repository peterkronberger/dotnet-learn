ILoggerFactory factory = provider.GetRequiredService<ILoggerFactory>();
ILogger logger = factory.CreateLogger("Imports.Nightly");
logger.LogInformation("Import finished");
