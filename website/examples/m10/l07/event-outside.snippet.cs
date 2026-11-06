Scanner scanner = new();
scanner.Found += name => Console.WriteLine(name);
scanner.Found = name => Console.WriteLine(name);
scanner.Found?.Invoke("from outside");

class Scanner
{
    public event Action<string>? Found;
}
