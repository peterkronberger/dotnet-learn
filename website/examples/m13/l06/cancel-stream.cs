// The attribute marks the parameter that receives the stream's token
using System.Runtime.CompilerServices;

using CancellationTokenSource source = new();
try
{
    await foreach (int number in CountAsync(source.Token))
    {
        Console.WriteLine($"consumed {number}");
        if (number == 2)
        {
            source.Cancel();
        }
    }
}
catch (OperationCanceledException ex)
{
    Console.WriteLine($"Stream ended with {ex.GetType().Name}");
}

async IAsyncEnumerable<int> CountAsync(
    [EnumeratorCancellation] CancellationToken token)
{
    for (int number = 1; number <= 5; number++)
    {
        await Task.Delay(1, token);
        yield return number;
    }
}
