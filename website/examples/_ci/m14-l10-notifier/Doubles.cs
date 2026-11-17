namespace Alerts.Tests;

// Stub: answers with a fixed value
public class StubNotifier : INotifier
{
    private readonly bool result;

    public StubNotifier(bool result)
    {
        this.result = result;
    }

    public bool Send(string message)
    {
        return result;
    }
}

// Spy: remembers every call
public class SpyNotifier : INotifier
{
    public List<string> Messages { get; } = [];

    public bool Send(string message)
    {
        Messages.Add(message);
        return true;
    }
}

// Fake: a simple working version that refuses a message it already has
public class InMemoryNotifier : INotifier
{
    private readonly HashSet<string> delivered = [];

    public bool Send(string message)
    {
        return delivered.Add(message);
    }
}
