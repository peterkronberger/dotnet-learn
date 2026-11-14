// Two workers take the same two locks in opposite order. This can hang.
Lock usersLock = new();
Lock ipsLock = new();

void AddFailure()
{
    lock (usersLock)
    {
        lock (ipsLock)
        {
            Console.WriteLine("Worker 1 has both locks");
        }
    }
}

void AddIp()
{
    lock (ipsLock)
    {
        lock (usersLock)
        {
            Console.WriteLine("Worker 2 has both locks");
        }
    }
}

Task first = Task.Run(AddFailure);
Task second = Task.Run(AddIp);
await Task.WhenAll(first, second);
