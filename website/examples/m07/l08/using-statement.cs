StringReader reader = new("first\nsecond");

// The using statement calls Dispose at the end of its block
using (reader)
{
    Console.WriteLine(reader.ReadLine());
    Console.WriteLine(reader.ReadLine());
}

// After Dispose the reader cannot be used any more
try
{
    reader.ReadLine();
}
catch (ObjectDisposedException ex)
{
    Console.WriteLine($"Disposed: {ex.GetType().Name}");
}
