// The caller sees the difference: null means "unreadable"
Console.WriteLine($"Failures: {ReadFailures("0")?.ToString() ?? "unreadable"}");
Console.WriteLine($"Failures: {ReadFailures("5x")?.ToString() ?? "unreadable"}");

// Read a failure count from text; null if the text is not a number
static int? ReadFailures(string text)
{
    if (int.TryParse(text, out int failures))
    {
        return failures;
    }
    return null;
}
