string[] inputs = ["8", "eight"];
foreach (string text in inputs)
{
    try
    {
        Console.WriteLine($"Parsed: {int.Parse(text)}");
    }
    catch (FormatException)
    {
        Console.WriteLine($"Not a number: {text}");
    }
    finally
    {
        Console.WriteLine($"Done with {text}");
    }
}
