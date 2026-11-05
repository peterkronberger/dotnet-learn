Console.WriteLine($"Result: {FirstNumber("12 34")}");
Console.WriteLine($"Result: {FirstNumber("")}");

int FirstNumber(string text)
{
    try
    {
        Console.WriteLine("Reading the first number");
        return int.Parse(text.Split(' ')[0]);
    }
    catch (FormatException)
    {
        Console.WriteLine("No number found");
        return -1;
    }
    finally
    {
        Console.WriteLine("Leaving FirstNumber");
    }
}
