// A method that finds an invalid value throws a fitting exception
string[] counts = ["12", "-5"];
foreach (string text in counts)
{
    try
    {
        Console.WriteLine($"Failures: {ParseFailures(text)}");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
    }
}

try
{
    CloseIncident(false);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
}

int ParseFailures(string text)
{
    int count = int.Parse(text);
    if (count < 0)
    {
        throw new ArgumentOutOfRangeException(
            "text", "A failure count cannot be negative.");
    }
    return count;
}

void CloseIncident(bool isOpen)
{
    if (!isOpen)
    {
        throw new InvalidOperationException("The incident is already closed.");
    }
    Console.WriteLine("Incident closed");
}
