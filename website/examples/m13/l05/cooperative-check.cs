// Cancel() is called in step 3. Which loop stops?
using CancellationTokenSource checkingSource = new();
try
{
    int done = await RunStepsAsync(checkingSource, checkToken: true);
    Console.WriteLine($"Loop with check: {done} steps");
}
catch (OperationCanceledException ex)
{
    Console.WriteLine($"Loop with check: {ex.GetType().Name}");
}

using CancellationTokenSource ignoringSource = new();
int allDone = await RunStepsAsync(ignoringSource, checkToken: false);
Console.WriteLine($"Loop without check: {allDone} steps, requested: {ignoringSource.IsCancellationRequested}");

async Task<int> RunStepsAsync(CancellationTokenSource source, bool checkToken)
{
    int done = 0;
    for (int step = 1; step <= 5; step++)
    {
        if (step == 3)
        {
            source.Cancel();
        }
        if (checkToken)
        {
            source.Token.ThrowIfCancellationRequested();
        }
        await Task.Delay(1);
        done++;
    }
    return done;
}
