string[] inputs = ["12", "x"];

foreach (string text in inputs)
{
    try
    {
        int number = int.Parse(text);
        Console.WriteLine($"number: {number}");
    }
    catch (FormatException)
    {
        Console.WriteLine($"not a number: {text}");
    }
}
