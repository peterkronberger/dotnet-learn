List<Plain> items = [new("bob"), new("ann")];
try
{
    items.Sort();
    Console.WriteLine("sorted");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Failed: {ex.GetType().Name}");
}
Console.WriteLine($"{items[0].Name} {items[1].Name}");

class Plain
{
    public string Name { get; }

    public Plain(string name)
    {
        Name = name;
    }
}
