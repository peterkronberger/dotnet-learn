// Wrap a low-level error in an own exception and keep it as InnerException
try
{
    Console.WriteLine(LogTools.ParseCount("7"));
    Console.WriteLine(LogTools.ParseCount("seven"));
}
catch (InvalidCountException ex)
{
    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
    Exception? cause = ex.InnerException;
    Console.WriteLine($"Cause: {cause?.GetType().Name}");
    Console.WriteLine($"Cause message: {cause?.Message}");
}

static class LogTools
{
    public static int ParseCount(string text)
    {
        try
        {
            return int.Parse(text);
        }
        catch (FormatException ex)
        {
            throw new InvalidCountException($"Not a count: {text}", ex);
        }
    }
}

class InvalidCountException : Exception
{
    public InvalidCountException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
