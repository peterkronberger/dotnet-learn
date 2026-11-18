using Microsoft.Extensions.Logging;

// A provider that writes to the console synchronously, so the lines keep their order
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
