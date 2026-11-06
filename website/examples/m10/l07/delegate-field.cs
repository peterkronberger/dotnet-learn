Scanner scanner = new();
scanner.Found += name => Console.WriteLine($"first: {name}");
scanner.Found += name => Console.WriteLine($"second: {name}");

scanner.Found = name => Console.WriteLine($"third: {name}");
scanner.Found?.Invoke("from outside");
scanner.Scan("disk");

class Scanner
{
    public Action<string>? Found;

    public void Scan(string name)
    {
        Found?.Invoke(name);
    }
}
