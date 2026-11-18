using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(logging => logging
    .SetMinimumLevel(LogLevel.Information)
    .AddProvider(new ConsoleLoggerProvider()));
services.AddTransient<LoginChecker>();

using var provider = services.BuildServiceProvider();
provider.GetRequiredService<LoginChecker>().Run();

class LoginChecker(ILogger<LoginChecker> logger)
{
    public void Run()
    {
        logger.LogTrace("Trace: very detailed steps");
        logger.LogDebug("Debug: information for developers");
        logger.LogInformation("Information: normal progress");
        logger.LogWarning("Warning: something unexpected, the app goes on");
        logger.LogError("Error: an operation failed");
        logger.LogCritical("Critical: the app cannot go on");
    }
}

// @fold-start
// A provider that writes to the console synchronously, so the lines keep their order
// (real apps use AddConsole() from the package Microsoft.Extensions.Logging.Console)
sealed class ConsoleLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new ConsoleLogger(categoryName);

    public void Dispose() { }

    sealed class ConsoleLogger(string category) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Console.WriteLine($"{logLevel}: {category}: {formatter(state, exception)}");
            if (exception is not null)
            {
                Console.WriteLine($"  {exception.GetType().Name}: {exception.Message}");
            }
        }
    }
}
// @fold-end
