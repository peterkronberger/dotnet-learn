using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddLogging(logging => logging
    .AddProvider(new ConsoleLoggerProvider(showData: false)));
services.AddTransient<FileImport>();

using var provider = services.BuildServiceProvider();
provider.GetRequiredService<FileImport>().Run();

class FileImport(ILogger<FileImport> logger)
{
    public void Run()
    {
        foreach (string file in new[] { "part-1.log", "part-2.log" })
        {
            using (logger.BeginScope("File {FileName}", file))
            {
                logger.LogInformation("Reading started");
                logger.LogInformation("Reading finished");
            }
        }
        logger.LogInformation("All files done");
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
