var counts = await Task.WhenAll(
    LoadCountAsync("alpha"),
    LoadCountAsync("beta"));
Console.WriteLine($"Counts: {counts[0]}, {counts[1]}");

using var cts = new CancellationTokenSource();
cts.Cancel();
try
{
    await Task.Delay(10, cts.Token);
    Console.WriteLine("Not reached");
}
catch (OperationCanceledException)
{
    Console.WriteLine("Delay was cancelled");
}

await foreach (var number in CountAsync(3))
{
    Console.WriteLine($"Item {number}");
}

await using (var resource = new Resource())
{
    Console.WriteLine("Using the resource");
}
Console.WriteLine("Done");

async Task<int> LoadCountAsync(string name)
{
    await Task.Delay(10);
    return name.Length;
}

async IAsyncEnumerable<int> CountAsync(int max)
{
    for (var i = 1; i <= max; i++)
    {
        await Task.Delay(1);
        yield return i;
    }
}

class Resource : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        await Task.Delay(1);
        Console.WriteLine("Resource disposed");
    }
}
