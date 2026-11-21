using SecLog.Model;

namespace SecLog.Tests.Doubles;

// Spy: subscribes to the event and remembers what it receives
public sealed class IncidentSpy
{
    public List<IncidentFoundEventArgs> Calls { get; } = [];

    public void Handle(object? sender, IncidentFoundEventArgs e)
    {
        Calls.Add(e);
    }
}
