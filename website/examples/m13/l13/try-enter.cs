// The main thread holds the lock. A second thread waits 50 ms for it.
Lock gate = new();
bool gotLock = true;

Thread waiter = new(() =>
{
    gotLock = gate.TryEnter(TimeSpan.FromMilliseconds(50));
    if (gotLock)
    {
        gate.Exit();
    }
});

gate.Enter();
waiter.Start();
waiter.Join();
gate.Exit();

Console.WriteLine($"Second thread got the lock: {gotLock}");
if (!gotLock)
{
    Console.WriteLine("Timeout: the lock was not free within 50 ms");
}
