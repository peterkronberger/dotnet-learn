// WhenAll returns the results in the order of its arguments
int[] results = await Task.WhenAll(SlowAsync(), FastAsync());
Console.WriteLine($"Results: {string.Join(", ", results)}");

async Task<int> SlowAsync()
{
    await Task.Delay(50);
    return 1;
}

async Task<int> FastAsync()
{
    await Task.Delay(10);
    return 2;
}
