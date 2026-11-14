// A producer writes log lines into a channel, a consumer reads them.
using System.Threading.Channels;

Channel<string> queue = Channel.CreateUnbounded<string>();

Task producer = Task.Run(async () =>
{
    string[] events = ["login-failed", "logout", "login-failed"];
    foreach (string produced in events)
    {
        await queue.Writer.WriteAsync(produced);
    }
    queue.Writer.Complete();
});

int failed = 0;
await foreach (string received in queue.Reader.ReadAllAsync())
{
    if (received == "login-failed")
    {
        failed++;
    }
}
await producer;
Console.WriteLine($"Failed: {failed}");
