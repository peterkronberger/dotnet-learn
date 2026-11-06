Feed feed = new();
Session session = new(feed);
feed.Raise();

session.Finish();
feed.Raise();

session.Close();
feed.Raise();

sealed class Feed
{
    public event EventHandler? Changed;

    public void Raise()
    {
        Console.WriteLine("raise");
        Changed?.Invoke(this, EventArgs.Empty);
    }
}

sealed class Session
{
    private readonly Feed feed;

    public Session(Feed feed)
    {
        this.feed = feed;
        feed.Changed += OnChanged;
    }

    public void Finish()
    {
        Console.WriteLine("session finished");
    }

    public void Close()
    {
        feed.Changed -= OnChanged;
        Console.WriteLine("session closed");
    }

    private void OnChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("session handles the event");
    }
}
