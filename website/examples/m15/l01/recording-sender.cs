// A test needs no console: it passes a sender that records the messages
RecordingSender recorder = new();
AlertService service = new(recorder);

service.Raise("bob", 5);
service.Raise("dave", 3);

Console.WriteLine($"Recorded: {recorder.Messages.Count}");
Console.WriteLine($"First: {recorder.Messages[0]}");

interface IAlertSender
{
    void Send(string text);
}

class RecordingSender : IAlertSender
{
    public List<string> Messages { get; } = [];

    public void Send(string text)
    {
        Messages.Add(text);
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
