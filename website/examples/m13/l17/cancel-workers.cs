// One token source stops all workers.
using CancellationTokenSource stop = new();
int stopped = 0;

List<Task> workers = [];
for (int number = 1; number <= 3; number++)
{
    workers.Add(Task.Run(async () =>
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stop.Token);
        }
        catch (OperationCanceledException)
        {
            Interlocked.Increment(ref stopped);
        }
    }));
}

stop.Cancel();
await Task.WhenAll(workers);
Console.WriteLine($"Workers stopped: {stopped}");
