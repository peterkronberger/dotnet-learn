using (Connection connection = new())
{
    Console.WriteLine("Working");
}

class Connection : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        await Task.Delay(1);
    }
}
