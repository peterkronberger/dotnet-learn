using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(logging => logging.AddProvider(new ConsoleLoggerProvider()));
services.AddSingleton<SingletonService>();
services.AddScoped<ScopedService>();
services.AddTransient<TransientService>();

using var provider = services.BuildServiceProvider();
var logger = provider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Singleton: same instance? {Same}",
    ReferenceEquals(provider.GetRequiredService<SingletonService>(), provider.GetRequiredService<SingletonService>()));
logger.LogInformation("Transient: same instance? {Same}",
    ReferenceEquals(provider.GetRequiredService<TransientService>(), provider.GetRequiredService<TransientService>()));

using (var scope = provider.CreateScope())
{
    var a = scope.ServiceProvider.GetRequiredService<ScopedService>();
    var b = scope.ServiceProvider.GetRequiredService<ScopedService>();
    logger.LogInformation("Scoped: same instance within a scope? {Same}", ReferenceEquals(a, b));

    using var other = provider.CreateScope();
    var c = other.ServiceProvider.GetRequiredService<ScopedService>();
    logger.LogInformation("Scoped: same instance across scopes? {Same}", ReferenceEquals(a, c));
}

provider.GetRequiredService<TransientService>().Greet("SecLog");

class SingletonService;

class ScopedService;

class TransientService(ILogger<TransientService> logger)
{
    public void Greet(string name) => logger.LogWarning("Hello, {Name}!", name);
}

// Minimal provider that writes to the console (the console logging package is not part of the playground).
sealed class ConsoleLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new ConsoleLogger(categoryName);

    public void Dispose() { }

    sealed class ConsoleLogger(string category) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            Console.WriteLine($"{logLevel}: {category}: {formatter(state, exception)}");
    }
}
