// A Task without a value: the caller only waits
Console.WriteLine("Order placed");
await PrepareAsync();
Console.WriteLine("Order ready");

async Task PrepareAsync()
{
    await Task.Delay(10);
    Console.WriteLine("Preparing");
}
