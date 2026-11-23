// This program also runs in the browser, which has only one thread
int callerThread = Environment.CurrentManagedThreadId;
Task<bool> task = CheckAsync();
Console.WriteLine("Caller continues");
Console.WriteLine($"Same thread before the await: {await task}");

async Task<bool> CheckAsync()
{
    bool same = Environment.CurrentManagedThreadId == callerThread;
    Console.WriteLine("CheckAsync started");
    await Task.Delay(10);
    return same;
}
