// A type with both interfaces: using calls Dispose, await using DisposeAsync
using (Connection plain = new())
{
    Console.WriteLine("Plain using");
}
await using (Connection waiting = new())
{
    Console.WriteLine("Await using");
}

class Connection : IDisposable, IAsyncDisposable
{
    public void Dispose()
    {
        Console.WriteLine("Dispose called");
    }

    public async ValueTask DisposeAsync()
    {
        await Task.Delay(1);
        Console.WriteLine("DisposeAsync called");
    }
}
