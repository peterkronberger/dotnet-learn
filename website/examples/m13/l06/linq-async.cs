// LINQ for async streams: Where and CountAsync work on IAsyncEnumerable
int even = await CountAsync().Where(number => number % 2 == 0).CountAsync();
Console.WriteLine($"Even numbers: {even}");

async IAsyncEnumerable<int> CountAsync()
{
    for (int number = 1; number <= 6; number++)
    {
        await Task.Delay(1);
        yield return number;
    }
}
