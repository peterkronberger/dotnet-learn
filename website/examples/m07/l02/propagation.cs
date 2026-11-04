try
{
    Console.WriteLine(DoubleOf("8"));
    Console.WriteLine(DoubleOf("eight"));
    Console.WriteLine("Both done");
}
catch (FormatException ex)
{
    Console.WriteLine($"Caught in the caller: {ex.GetType().Name}");
}
Console.WriteLine("Program continues");

int DoubleOf(string text)
{
    Console.WriteLine($"Reading {text}");
    int number = int.Parse(text);
    Console.WriteLine($"Read {number}");
    return number * 2;
}
