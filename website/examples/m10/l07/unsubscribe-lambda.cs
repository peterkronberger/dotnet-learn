Scanner scanner = new();
scanner.Found += name => Console.WriteLine($"log: {name}");
scanner.Found -= name => Console.WriteLine($"log: {name}");

Action<string> counter = name => Console.WriteLine($"count: {name}");
scanner.Found += counter;
scanner.Scan("disk");

scanner.Found -= counter;
scanner.Scan("cpu");

class Scanner
{
    public event Action<string>? Found;

    public void Scan(string name)
    {
        Found?.Invoke(name);
    }
}
