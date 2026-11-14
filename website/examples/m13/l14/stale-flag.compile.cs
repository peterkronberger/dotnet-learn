// The worker may never see the new value of the flag. This can hang.
Worker worker = new();
Thread thread = new(worker.Run);
thread.Start();

Thread.Sleep(100);
worker.Stop = true;
thread.Join();
Console.WriteLine("Worker stopped");

class Worker
{
    public bool Stop;

    public void Run()
    {
        while (!Stop)
        {
        }
    }
}
