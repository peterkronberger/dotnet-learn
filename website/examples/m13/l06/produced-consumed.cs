// The stream prints "produced", the loop prints "consumed"
await foreach (int number in CountAsync())
{
    Console.WriteLine($"consumed {number}");
}

async IAsyncEnumerable<int> CountAsync()
{
    for (int number = 1; number <= 3; number++)
    {
        await Task.Delay(1);
        Console.WriteLine($"produced {number}");
        yield return number;
    }
}
