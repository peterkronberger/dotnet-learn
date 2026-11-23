// Give up waiting after 50 ms. The slow task itself keeps running.
Task<string> slow = LoadRemoteAsync();

try
{
    string text = await slow.WaitAsync(TimeSpan.FromMilliseconds(50));
    Console.WriteLine($"Loaded: {text}");
}
catch (TimeoutException)
{
    Console.WriteLine("Gave up waiting");
}

Console.WriteLine($"Task completed: {slow.IsCompleted}");

async Task<string> LoadRemoteAsync()
{
    await Task.Delay(5000);
    return "remote";
}
