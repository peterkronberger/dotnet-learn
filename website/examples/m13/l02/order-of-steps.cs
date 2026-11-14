// Watch the order of the output lines
Console.WriteLine("1 before the call");
Task<int> pending = LoadAsync();
Console.WriteLine("3 after the call");
int value = await pending;
Console.WriteLine($"5 value is {value}");

async Task<int> LoadAsync()
{
    Console.WriteLine("2 inside, before the await");
    await Task.Delay(10);
    Console.WriteLine("4 inside, after the await");
    return 7;
}
