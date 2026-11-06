Scanner quiet = new();
quiet.Scan("nobody listens");

Scanner scanner = new();
scanner.Found += PrintPlain;
scanner.Found += name => Console.WriteLine($"upper: {name.ToUpper()}");

scanner.Scan("disk");
scanner.Found -= PrintPlain;
scanner.Scan("cpu");

static void PrintPlain(string name)
{
    Console.WriteLine($"plain: {name}");
}

class Scanner
{
    public event Action<string>? Found;

    public void Scan(string name)
    {
        Console.WriteLine($"scanning {name}");
        Found?.Invoke(name);
    }
}
