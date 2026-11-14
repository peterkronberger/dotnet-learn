using System.Threading.Channels;

Channel<string> queue = Channel.CreateBounded<string>(2);
int failed = 0;

Task producer = Task.Run(async () =>
{
    string[] events = ["login-failed", "logout", "login-failed", "login-failed"];
    foreach (string produced in events)
    {
        await queue.Writer.WriteAsync(produced);
    }
    queue.Writer.Complete();
});

Task consumer = Task.Run(async () =>
{
    await foreach (string received in queue.Reader.ReadAllAsync())
    {
        if (received == "login-failed")
        {
            Interlocked.Increment(ref failed);
        }
    }
});

await producer;
await consumer;
Console.WriteLine($"Failed: {failed}");
