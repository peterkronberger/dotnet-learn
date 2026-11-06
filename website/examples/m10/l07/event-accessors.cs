Scanner scanner = new();
scanner.Found += name => Console.WriteLine($"one: {name}");
scanner.Found += name => Console.WriteLine($"two: {name}");
scanner.Scan("disk");

class Scanner
{
    private readonly List<Action<string>> handlers = [];

    public event Action<string> Found
    {
        add
        {
            Console.WriteLine("add called");
            handlers.Add(value);
        }
        remove
        {
            Console.WriteLine("remove called");
            handlers.Remove(value);
        }
    }

    public void Scan(string name)
    {
        foreach (Action<string> handler in handlers)
        {
            handler(name);
        }
    }
}
