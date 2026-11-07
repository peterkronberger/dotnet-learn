using Session session = new("audit");
Console.WriteLine($"working in {session.Name}");

class Session(string name) : IDisposable
{
    public string Name => name;

    public void Dispose()
    {
        Console.WriteLine($"closed {name}");
    }
}
