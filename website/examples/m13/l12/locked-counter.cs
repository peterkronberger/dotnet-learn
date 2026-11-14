// Two tasks each add 1000 to a shared counter. The lock makes it exact.
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

Task first = Task.Run(AddThousand);
Task second = Task.Run(AddThousand);

await Task.WhenAll(first, second);
Console.WriteLine($"Count: {count}");
