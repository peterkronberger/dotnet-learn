// volatile does not make count++ atomic. The result can be below 2000.
Counter counter = new();

Task first = Task.Run(counter.AddThousand);
Task second = Task.Run(counter.AddThousand);

await Task.WhenAll(first, second);
Console.WriteLine($"Count: {counter.Count}");

class Counter
{
    private volatile int _count;

    public int Count => _count;

    public void AddThousand()
    {
        for (int i = 0; i < 1000; i++)
        {
            _count++;
        }
    }
}
