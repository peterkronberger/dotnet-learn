// An event handler that starts async work and stores the task
Alarm alarm = new();
List<Task> pending = [];
alarm.Raised += (sender, e) => pending.Add(SendAsync());
alarm.Raise();
alarm.Raise();
Console.WriteLine($"Handlers returned, tasks stored: {pending.Count}");
await Task.WhenAll(pending);
Console.WriteLine($"All tasks completed: {pending.All(t => t.IsCompleted)}");

async Task SendAsync()
{
    await Task.Delay(10);
}

class Alarm
{
    public event EventHandler? Raised;

    public void Raise()
    {
        Raised?.Invoke(this, EventArgs.Empty);
    }
}
