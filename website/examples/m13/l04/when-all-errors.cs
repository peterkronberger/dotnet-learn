// Two tasks fail: await throws one error, the task holds both
Task<int> first = FailAsync("first", 10);
Task<int> second = FailAsync("second", 100);
Task<int[]> all = Task.WhenAll(first, second);
try
{
    await all;
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}
Console.WriteLine($"Errors in the task: {all.Exception?.InnerExceptions.Count}");

async Task<int> FailAsync(string name, int delay)
{
    await Task.Delay(delay);
    throw new InvalidOperationException($"{name} failed");
}
