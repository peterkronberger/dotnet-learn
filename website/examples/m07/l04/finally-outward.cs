try
{
    Console.WriteLine(Inner("x"));
}
catch (FormatException ex)
{
    Console.WriteLine($"Caught in the caller: {ex.GetType().Name}");
}
Console.WriteLine("Program continues");

int Inner(string text)
{
    try
    {
        return int.Parse(text);
    }
    finally
    {
        Console.WriteLine("Inner: finally runs on the way out");
    }
}
