// A task that is already complete does not make the method wait
Task<int> quick = QuickAsync();
Console.WriteLine($"quick completed: {quick.IsCompleted}");
Task<int> slow = SlowAsync();
Console.WriteLine($"slow completed: {slow.IsCompleted}");
Console.WriteLine($"Sum: {await quick + await slow}");

async Task<int> QuickAsync()
{
    return await Task.FromResult(1);
}

async Task<int> SlowAsync()
{
    await Task.Delay(10);
    return 2;
}
