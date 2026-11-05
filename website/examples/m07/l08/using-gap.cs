// Make Dispose run when Read ends, also if the work throws.
// Replace the try/finally with a using declaration.
// Wanted output: Working, Dispose log, Done
Read();
Console.WriteLine("Done");

void Read()
{
    Resource log = new("log");
    try
    {
        Console.WriteLine("Working");
    }
    finally
    {
        log.Dispose();
    }
}

class Resource(string name) : IDisposable
{
    public void Dispose()
    {
        Console.WriteLine($"Dispose {name}");
    }
}
