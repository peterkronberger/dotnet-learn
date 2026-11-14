// Fill the gaps: an async method that returns an int
int length = await CountAsync("tasks");
Console.WriteLine($"Length: {length}");

async Task<int> CountAsync(string text)
{
    await Task.Delay(10);
    return text.Length;
}
