// Fill the gaps: store the task of an async method in a handler
Alarm alarm = new();
List<Task> pending = [];
alarm.Raised += (sender, e) => pending.Add(SendAsync());
alarm.Raise();
await Task.WhenAll(pending);
Console.WriteLine($"Tasks: {pending.Count}");

async Task SendAsync()
{
    await Task.Delay(1);
}

class Alarm
{
    public event EventHandler? Raised;

    public void Raise()
    {
        Raised?.Invoke(this, EventArgs.Empty);
    }
}
