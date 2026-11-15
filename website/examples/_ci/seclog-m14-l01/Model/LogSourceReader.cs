namespace SecLog.Model;

/// <summary>Reads one log source and releases it asynchronously.</summary>
sealed class LogSourceReader : IAsyncDisposable
{
    private readonly string[] _lines;

    public LogSourceReader(string name, string[] lines)
    {
        Name = name;
        _lines = lines;
    }

    public string Name { get; }

    public int CountFailures()
    {
        return _lines.Count(line => line.Contains("login-failed"));
    }

    public async ValueTask DisposeAsync()
    {
        await Task.Delay(1);
        Console.WriteLine($"Reader closed: {Name}");
    }
}
