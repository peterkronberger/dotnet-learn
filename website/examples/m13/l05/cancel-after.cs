// CancelAfter signals the token later: 50 ms against a delay of 5 s
using CancellationTokenSource source = new();
source.CancelAfter(TimeSpan.FromMilliseconds(50));
Task delay = Task.Delay(5000, source.Token);

try
{
    await delay;
}
catch (OperationCanceledException)
{
    Console.WriteLine($"Canceled: {delay.IsCanceled}, faulted: {delay.IsFaulted}");
}
