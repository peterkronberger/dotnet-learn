// Fill the gaps: which line comes first?
Console.WriteLine("A");
Task pending = WorkAsync();
Console.WriteLine("C");
await pending;
Console.WriteLine("E");

async Task WorkAsync()
{
    Console.WriteLine("B");
    await Task.Delay(10);
    Console.WriteLine("D");
}
