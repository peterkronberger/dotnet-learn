// Print the value only when it has the type int
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
