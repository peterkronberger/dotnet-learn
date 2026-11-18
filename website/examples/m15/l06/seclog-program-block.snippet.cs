// Reject invalid lines: the validator logs every line it rejects
LineValidator validator = provider.GetRequiredService<LineValidator>();
ILogger runLogger = provider.GetRequiredService<ILoggerFactory>().CreateLogger("SecLog.Run");
using (runLogger.BeginScope("File {FileName}", "sample.log"))
{
    validator.TryParse(sampleLog[1], 2);
    validator.TryParse(brokenLines[0], 3);
    validator.TryParse(brokenLines[1], 4);
    runLogger.LogInformation("Validation finished");
}
