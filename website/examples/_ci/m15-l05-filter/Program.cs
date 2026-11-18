using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(logging => logging
    .SetMinimumLevel(LogLevel.Information)
    .AddFilter("Notifier", LogLevel.Warning)
    .AddProvider(new ConsoleLoggerProvider()));
services.AddTransient<Importer>();
services.AddTransient<Notifier>();

using var provider = services.BuildServiceProvider();
provider.GetRequiredService<Importer>().Run();
provider.GetRequiredService<Notifier>().Send("alice");

class Importer(ILogger<Importer> logger)
{
    public void Run() => logger.LogInformation("Import finished");
}

class Notifier(ILogger<Notifier> logger)
{
    public void Send(string user)
    {
        logger.LogInformation("Sending a message to {User}", user);
        try
        {
            throw new InvalidOperationException("mailbox is full");
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Sending to {User} failed", user);
        }
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
