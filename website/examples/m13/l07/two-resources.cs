// Release two resources: the order is reversed
await using Connection first = new("first");
await using Connection second = new("second");
Console.WriteLine("Working with both");

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
