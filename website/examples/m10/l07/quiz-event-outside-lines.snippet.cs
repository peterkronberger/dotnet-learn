Scanner scanner = new();

static void Print(string name)
{
    Console.WriteLine(name);
}

class Scanner
{
    public event Action<string>? Found;
}
