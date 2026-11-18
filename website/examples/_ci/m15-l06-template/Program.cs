using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(logging => logging.AddProvider(new ConsoleLoggerProvider()));
services.AddTransient<LoginAudit>();

using var provider = services.BuildServiceProvider();
provider.GetRequiredService<LoginAudit>().Run();

class LoginAudit(ILogger<LoginAudit> logger)
{
    public void Run()
    {
        string user = "bob";
        string ip = "203.0.113.7";
        int failures = 5;

        logger.LogWarning("User {User} from {Ip} failed {Failures} times",
            user, ip, failures);
        logger.LogWarning("Attempts by {Ip} for {User}", user, ip);
        logger.LogWarning($"User {user} from {ip} failed {failures} times");
    }
}

// @fold-start
// A provider that writes to the console synchronously, so the lines keep their order.
// It also prints the data the logger gets: the template and the named values.
sealed class ConsoleLoggerProvider(bool showData = true)
    : ILoggerProvider, ISupportExternalScope
{
    IExternalScopeProvider scopes = new LoggerExternalScopeProvider();

    public ILogger CreateLogger(string categoryName) =>
        new ConsoleLogger(categoryName, () => scopes, showData);

    public void SetScopeProvider(IExternalScopeProvider scopeProvider) =>
        scopes = scopeProvider;

    public void Dispose() { }

    sealed class ConsoleLogger(string category,
        Func<IExternalScopeProvider> getScopes, bool showData) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
            getScopes().Push(state);

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string prefix = "";
            getScopes().ForEachScope((scope, _) => prefix += $"[{scope}] ", 0);
            Console.WriteLine($"{logLevel}: {category}: {prefix}{formatter(state, exception)}");
            if (showData && state is IEnumerable<KeyValuePair<string, object?>> properties)
            {
                foreach (var (key, value) in properties)
                {
                    Console.WriteLine(key == "{OriginalFormat}"
                        ? $"  template: {value}"
                        : $"  {key} = {value}");
                }
            }
        }
    }
}
// @fold-end
