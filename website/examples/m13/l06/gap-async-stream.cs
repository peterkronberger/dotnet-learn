// Fill the gaps: write and consume an async stream
int sum = 0;
await foreach (int number in NumbersAsync())
{
    sum += number;
}
Console.WriteLine($"Sum: {sum}");

async IAsyncEnumerable<int> NumbersAsync()
{
    for (int number = 1; number <= 3; number++)
    {
        await Task.Delay(1);
        yield return number;
    }
}
