using Microsoft.Extensions.Logging;

// A provider that writes to the console synchronously, so the lines keep their order.
// It also writes the scopes that are active when a message is logged.
sealed class ConsoleLoggerProvider : ILoggerProvider, ISupportExternalScope
{
    IExternalScopeProvider scopes = new LoggerExternalScopeProvider();

    public ILogger CreateLogger(string categoryName) =>
        new ConsoleLogger(categoryName, () => scopes);

    public void SetScopeProvider(IExternalScopeProvider scopeProvider) =>
        scopes = scopeProvider;

    public void Dispose() { }

    sealed class ConsoleLogger(string category, Func<IExternalScopeProvider> getScopes)
        : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
            getScopes().Push(state);

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string prefix = "";
            getScopes().ForEachScope((scope, _) => prefix += $"[{scope}] ", 0);
            Console.WriteLine($"{logLevel}: {category}: {prefix}{formatter(state, exception)}");
        }
    }
}
