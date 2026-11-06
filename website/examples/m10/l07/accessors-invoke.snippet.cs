Scanner scanner = new();
scanner.Scan("disk");

class Scanner
{
    private readonly List<Action<string>> handlers = [];

    public event Action<string> Found
    {
        add { handlers.Add(value); }
        remove { handlers.Remove(value); }
    }

    public void Scan(string name)
    {
        Found?.Invoke(name);
    }
}
