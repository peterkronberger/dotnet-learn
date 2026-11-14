// Fill the gaps: signal the cancellation and pass the token down
using CancellationTokenSource source = new();
source.Cancel();

try
{
    await WaitAsync(source.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Canceled");
}

async Task WaitAsync(CancellationToken token)
{
    await Task.Delay(1000, token);
}
