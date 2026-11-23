// Print the number only when the value really is an int
object[] values = { 42, "forty-two", 4.2 };

foreach (object value in values)
{
    if (value is int number)
    {
        Console.WriteLine($"int: {number}");
    }
    else
    {
        Console.WriteLine("not an int");
    }
}
