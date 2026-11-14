// The lock is released when the block ends with an exception.
Lock gate = new();

try
{
    lock (gate)
    {
        throw new InvalidOperationException("failed inside the lock");
    }
}
catch (InvalidOperationException)
{
    Console.WriteLine("Exception caught");
}

Console.WriteLine($"Held by this thread: {gate.IsHeldByCurrentThread}");
lock (gate)
{
    Console.WriteLine("Entered again");
}
