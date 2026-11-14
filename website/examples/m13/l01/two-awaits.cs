// await in top-level statements: two waits, one after the other
int first = await ReadValueAsync(10);
int second = await ReadValueAsync(20);
Console.WriteLine($"Sum: {first + second}");

async Task<int> ReadValueAsync(int value)
{
    await Task.Delay(10);
    return value;
}
