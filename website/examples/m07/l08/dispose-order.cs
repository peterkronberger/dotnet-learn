try
{
    Work();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}

void Work()
{
    using Resource first = new("first");
    using Resource second = new("second");
    Console.WriteLine("Working");
    throw new InvalidOperationException("Step failed");
}

class Resource(string name) : IDisposable
{
    public void Dispose()
    {
        Console.WriteLine($"Dispose {name}");
    }
}
