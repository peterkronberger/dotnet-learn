// Tell a cancellation from a real error with a when filter
using CancellationTokenSource source = new();
source.Cancel();

await TryWorkAsync("cancelled call", fail: false, source.Token);
await TryWorkAsync("failing call", fail: true);
await TryWorkAsync("normal call", fail: false);

async Task TryWorkAsync(string label, bool fail, CancellationToken token = default)
{
    try
    {
        await WorkAsync(fail, token);
        Console.WriteLine($"{label}: done");
    }
    catch (OperationCanceledException) when (token.IsCancellationRequested)
    {
        Console.WriteLine($"{label}: canceled");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{label}: failed with {ex.GetType().Name}");
    }
}

async Task WorkAsync(bool fail, CancellationToken token)
{
    await Task.Delay(10, token);
    if (fail)
    {
        throw new InvalidOperationException("Work failed");
    }
}
