// Composition root: all objects are created and connected here
IAlertSender console = new ConsoleSender();
AlertService first = new(console);
first.Raise("bob", 5);

IAlertSender prefixed = new PrefixSender("[ALERT]");
AlertService second = new(prefixed);
second.Raise("dave", 3);

interface IAlertSender
{
    void Send(string text);
}

class ConsoleSender : IAlertSender
{
    public void Send(string text)
    {
        Console.WriteLine($"SEND: {text}");
    }
}

class PrefixSender : IAlertSender
{
    private readonly string prefix;

    public PrefixSender(string prefix)
    {
        this.prefix = prefix;
    }

    public void Send(string text)
    {
        Console.WriteLine($"{prefix} {text}");
    }
}

class AlertService
{
    private readonly IAlertSender sender;

    public AlertService(IAlertSender sender)
    {
        this.sender = sender;
    }

    public void Raise(string user, int failures)
    {
        sender.Send($"{user} failed {failures} times");
    }
}
