// WhenAny returns the first task that is complete
Task<string> cached = Task.FromResult("cache");
Task<string> remote = LoadRemoteAsync();
Task<string> winner = await Task.WhenAny(cached, remote);
Console.WriteLine($"First result: {await winner}");
Console.WriteLine($"Remote completed: {remote.IsCompleted}");

async Task<string> LoadRemoteAsync()
{
    await Task.Delay(5000);
    return "remote";
}
