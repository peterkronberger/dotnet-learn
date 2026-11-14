// A semaphore lets at most two jobs run at the same time.
using SemaphoreSlim limit = new(2);
int running = 0;
int tooMany = 0;

async Task RunJobAsync()
{
    await limit.WaitAsync();
    try
    {
        int now = Interlocked.Increment(ref running);
        if (now > 2)
        {
            Interlocked.Increment(ref tooMany);
        }
        await Task.Delay(20);
        Interlocked.Decrement(ref running);
    }
    finally
    {
        limit.Release();
    }
}

List<Task> jobs = [];
for (int number = 1; number <= 6; number++)
{
    jobs.Add(RunJobAsync());
}
await Task.WhenAll(jobs);
Console.WriteLine($"Jobs finished: {jobs.Count}");
Console.WriteLine($"More than 2 at the same time: {tooMany > 0}");
