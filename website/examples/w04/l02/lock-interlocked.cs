// Two threads add to two shared counters. Both counters stay exact.
Lock gate = new();
int total = 0;
int hits = 0;

void Work()
{
    for (int i = 0; i < 1000; i++)
    {
        lock (gate)
        {
            total++;
        }
        Interlocked.Increment(ref hits);
    }
}

Thread first = new(Work);
Thread second = new(Work);
first.Start();
second.Start();
first.Join();
second.Join();

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Hits: {hits}");
