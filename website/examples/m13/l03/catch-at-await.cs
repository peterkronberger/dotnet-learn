// The error of an async method arrives at the await, not at the call
Console.WriteLine("1 before the call");
Task<int> pending = ReadNumberAsync();
Console.WriteLine("2 after the call");
try
{
    int number = await pending;
    Console.WriteLine($"3 number is {number}");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"4 caught {ex.GetType().Name}");
}

async Task<int> ReadNumberAsync()
{
    await Task.Delay(10);
    throw new InvalidOperationException("No number available");
}
