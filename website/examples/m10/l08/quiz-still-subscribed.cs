Bell bell = new();
EventHandler<EventArgs> handler = (sender, e) => Console.WriteLine("ding");
bell.Rang += handler;
bell.Ring();

handler = null!;
bell.Ring();

sealed class Bell
{
    public event EventHandler<EventArgs>? Rang;

    public void Ring()
    {
        Rang?.Invoke(this, EventArgs.Empty);
    }
}
