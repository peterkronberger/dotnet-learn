// Predict the order of the lines
try
{
    await using (Connection connection = new("web"))
    {
        Console.WriteLine("Working");
        throw new InvalidOperationException("broken");
    }
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}

class Connection : IAsyncDisposable
{
    public Connection(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public async ValueTask DisposeAsync()
    {
        await Task.Delay(1);
        Console.WriteLine($"Closed {Name}");
    }
}
