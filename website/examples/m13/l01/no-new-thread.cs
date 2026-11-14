// This program also runs in the browser, which has only one thread
string coffee = await BrewAsync("coffee");
string tea = await BrewAsync("tea");
Console.WriteLine($"{coffee} and {tea}");

async Task<string> BrewAsync(string drink)
{
    await Task.Delay(10);
    return $"hot {drink}";
}
