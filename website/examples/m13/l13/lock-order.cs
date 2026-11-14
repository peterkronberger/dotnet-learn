// Two workers need both locks. Both take them in the same order.
Lock usersLock = new();
Lock ipsLock = new();
int users = 0;
int ips = 0;

void Record()
{
    for (int i = 0; i < 1000; i++)
    {
        lock (usersLock)
        {
            lock (ipsLock)
            {
                users++;
                ips++;
            }
        }
    }
}

Task first = Task.Run(Record);
Task second = Task.Run(Record);
await Task.WhenAll(first, second);
Console.WriteLine($"Users: {users}, IPs: {ips}");
