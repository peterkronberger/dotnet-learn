Watcher watcher = new();
watcher.Found += OnFound;

static void OnFound(string name)
{
    Console.WriteLine(name);
}

sealed class Watcher
{
    public event EventHandler<EventArgs>? Found;
}
