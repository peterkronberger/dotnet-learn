// Limit the waiting time: 50 ms against a delay of 5 s
try
{
    string text = await LoadRemoteAsync().WaitAsync(TimeSpan.FromMilliseconds(50));
    Console.WriteLine($"Loaded: {text}");
}
catch (TimeoutException ex)
{
    Console.WriteLine($"Caught {ex.GetType().Name}");
}

async Task<string> LoadRemoteAsync()
{
    await Task.Delay(5000);
    return "remote";
}
