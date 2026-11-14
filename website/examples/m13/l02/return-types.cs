// Task has no result, Task<int> has an int result
await PrintAsync("report");
int count = await CountAsync("report");
Console.WriteLine($"Letters: {count}");

async Task PrintAsync(string text)
{
    await Task.Delay(10);
    Console.WriteLine($"Printed {text}");
}

async Task<int> CountAsync(string text)
{
    await Task.Delay(10);
    return text.Length;
}
