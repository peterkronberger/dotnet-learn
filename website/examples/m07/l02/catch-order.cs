string[] inputs = ["5", "abc", "99999999999", "0"];
foreach (string text in inputs)
{
    try
    {
        Console.WriteLine($"{text} -> {100 / int.Parse(text)}");
    }
    catch (FormatException)
    {
        Console.WriteLine($"{text} -> FormatException");
    }
    catch (OverflowException)
    {
        Console.WriteLine($"{text} -> OverflowException");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{text} -> other: {ex.GetType().Name}");
    }
}
