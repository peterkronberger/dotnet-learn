Process("12x");
Console.WriteLine("After the call");

void Process(string text)
{
    try
    {
        int number = int.Parse(text);
        Console.WriteLine(number);
    }
    finally
    {
        Console.WriteLine("Cleanup");
    }
}
