// A cancelled token makes Task.Delay end with an exception
using CancellationTokenSource source = new();
source.Cancel();
Console.WriteLine($"Cancellation requested: {source.Token.IsCancellationRequested}");

try
{
    await Task.Delay(5000, source.Token);
    Console.WriteLine("Delay finished");
}
catch (OperationCanceledException ex)
{
    Console.WriteLine($"Delay ended with {ex.GetType().Name}");
}
