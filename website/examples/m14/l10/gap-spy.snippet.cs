public class SpyNotifier : INotifier
{
    public List<string> Messages { get; } = [];

    public bool Send(string message)
    {
        Messages.___(message);
        return ___;
    }
}
