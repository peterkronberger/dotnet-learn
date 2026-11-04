string[] inputs = ["12", "", "abc"];
foreach (string text in inputs)
{
    try
    {
        Console.WriteLine(int.Parse(text));
    }
    catch (FormatException) when (text.Length == 0)
    {
        Console.WriteLine("Empty text");
    }
    catch (FormatException)
    {
        Console.WriteLine($"Not a number: {text}");
    }
}
