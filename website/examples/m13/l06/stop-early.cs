// break ends the loop: the stream is closed and produces nothing more
await foreach (int number in CountAsync())
{
    Console.WriteLine($"consumed {number}");
    if (number == 2)
    {
        break;
    }
}
Console.WriteLine("Loop finished");

async IAsyncEnumerable<int> CountAsync()
{
    try
    {
        for (int number = 1; number <= 5; number++)
        {
            await Task.Delay(1);
            yield return number;
        }
    }
    finally
    {
        Console.WriteLine("Stream closed");
    }
}
