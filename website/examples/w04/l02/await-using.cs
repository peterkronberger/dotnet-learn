await using (Connection connection = new())
{
    Console.WriteLine("Working");
}
Console.WriteLine("Done");

class Connection : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        await Task.Delay(1);
        Console.WriteLine("Closed");
    }
}
