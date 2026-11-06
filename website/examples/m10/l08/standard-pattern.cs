LoginWatcher watcher = new("gate");
watcher.AttemptFailed += (sender, e) =>
{
    if (sender is LoginWatcher source)
    {
        Console.WriteLine($"{source.Name}: {e.User} failed {e.Count} times");
    }
};
watcher.AttemptFailed += (sender, e) => Console.WriteLine($"log {e.User}");

watcher.Report("bob", 3);
watcher.Report("dave", 1);

sealed class AttemptEventArgs : EventArgs
{
    public AttemptEventArgs(string user, int count)
    {
        User = user;
        Count = count;
    }

    public string User { get; }
    public int Count { get; }
}

sealed class LoginWatcher
{
    public LoginWatcher(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public event EventHandler<AttemptEventArgs>? AttemptFailed;

    public void Report(string user, int count)
    {
        OnAttemptFailed(new AttemptEventArgs(user, count));
    }

    private void OnAttemptFailed(AttemptEventArgs e)
    {
        AttemptFailed?.Invoke(this, e);
    }
}
