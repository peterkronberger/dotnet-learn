// The caller cannot tell "0 failures" from "unreadable text"
Console.WriteLine($"Failures: {ReadFailures("0")}");
Console.WriteLine($"Failures: {ReadFailures("5x")}");

// Read a failure count from text; errors are swallowed
static int ReadFailures(string text)
{
    try
    {
        return int.Parse(text);
    }
    catch
    {
    }
    return 0;
}
