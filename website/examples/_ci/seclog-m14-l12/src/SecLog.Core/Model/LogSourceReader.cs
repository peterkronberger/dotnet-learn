namespace SecLog.Model;

/// <summary>Reads one log source and releases it asynchronously.</summary>
public sealed class LogSourceReader : IAsyncDisposable
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

    /// <summary>Counts the failed logins, one short wait per line.</summary>
    /// <param name="token">Cancels the count.</param>
    /// <returns>The number of lines with a failed login.</returns>
    /// <exception cref="OperationCanceledException">The token was canceled.</exception>
    public async Task<int> CountFailuresAsync(CancellationToken token)
    {
        int count = 0;
        foreach (string line in _lines)
        {
            await Task.Delay(1, token);
            if (line.Contains("login-failed"))
            {
                count++;
            }
        }
        return count;
    }

    public async ValueTask DisposeAsync()
    {
        await Task.Delay(1);
        Console.WriteLine($"Reader closed: {Name}");
    }
}
