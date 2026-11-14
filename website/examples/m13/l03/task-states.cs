// One task succeeds, one fails, and nobody awaits the failed one
Task<int> good = LoadNumberAsync(false);
Task<int> lost = LoadNumberAsync(true);
await Task.Delay(100);
Console.WriteLine($"good: faulted {good.IsFaulted}, ok {good.IsCompletedSuccessfully}");
Console.WriteLine($"lost: faulted {lost.IsFaulted}, ok {lost.IsCompletedSuccessfully}");
Console.WriteLine($"The program goes on, good is {await good}");

async Task<int> LoadNumberAsync(bool fail)
{
    await Task.Delay(10);
    if (fail)
    {
        throw new InvalidOperationException("Load failed");
    }
    return 42;
}
