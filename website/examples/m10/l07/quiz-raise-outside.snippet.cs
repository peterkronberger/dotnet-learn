Scanner scanner = new();
scanner.Found?.Invoke("x");

class Scanner
{
    public event Action<string>? Found;
}
