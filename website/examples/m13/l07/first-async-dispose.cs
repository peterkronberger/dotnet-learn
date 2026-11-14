// Release a resource with await using
await using (Connection connection = new("web"))
{
    Console.WriteLine($"Working with {connection.Name}");
}
Console.WriteLine("After the block");

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
