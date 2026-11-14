// Two SecLog workers update the user table and the IP table.
Lock usersLock = new();
Lock ipsLock = new();

void AddFailure(string user, string ip)
{
    lock (usersLock)
    {
        lock (ipsLock)
        {
            Console.WriteLine($"Add {user} {ip}");
        }
    }
}

void RemoveEntry(string user, string ip)
{
    lock (ipsLock)
    {
        lock (usersLock)
        {
            Console.WriteLine($"Remove {user} {ip}");
        }
    }
}
