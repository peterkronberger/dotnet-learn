namespace Alerts;

public interface INotifier
{
    // Sends a message. Returns true if it was delivered.
    bool Send(string message);
}
