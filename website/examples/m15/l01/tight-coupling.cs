// The service creates its sender itself with new
AlertService service = new();
service.Raise("bob", 5);

class ConsoleSender
{
    public void Send(string text)
    {
        Console.WriteLine($"SEND: {text}");
    }
}

class AlertService
{
    private readonly ConsoleSender sender = new();

    public void Raise(string user, int failures)
    {
        sender.Send($"{user} failed {failures} times");
    }
}
