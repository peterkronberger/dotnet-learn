// The writes are locked, but the read at the end is not part of any lock.
Lock countLock = new();
int count = 0;

void AddThousand()
{
    for (int i = 0; i < 1000; i++)
    {
        lock (countLock)
        {
            count++;
        }
    }
}

Task worker = Task.Run(AddThousand);
Console.WriteLine($"Current count: {count}");
await worker;
