// An async stream: each line arrives after a short wait
await foreach (string line in ReadLinesAsync())
{
    Console.WriteLine($"Got: {line}");
}
Console.WriteLine("Stream finished");

async IAsyncEnumerable<string> ReadLinesAsync()
{
    string[] lines = ["first", "second", "third"];
    foreach (string line in lines)
    {
        await Task.Delay(1);
        yield return line;
    }
}
