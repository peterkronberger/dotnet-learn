Clock clock = new();
clock.Ticked += (sender, e) =>
    Console.WriteLine($"tick, empty: {e == EventArgs.Empty}");
clock.Tick();

sealed class Clock
{
    public event EventHandler? Ticked;

    public void Tick()
    {
        Ticked?.Invoke(this, EventArgs.Empty);
    }
}
