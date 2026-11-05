string[] texts = ["8080", "http", "70000"];
foreach (string text in texts)
{
    if (TryParsePort(text, out int port))
    {
        Console.WriteLine($"Port {port}");
    }
    else
    {
        Console.WriteLine($"Not a port: {text}");
    }
}

// Returns true and sets port, or returns false and sets port to 0
bool TryParsePort(string text, out int port)
{
    if (!int.TryParse(text, out port))
    {
        return false;
    }
    if (port < 1 || port > 65535)
    {
        port = 0;
        return false;
    }
    return true;
}
