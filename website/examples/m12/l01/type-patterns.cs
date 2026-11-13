// Test each value against patterns
object?[] values = ["hello", 42, 3.5, null];
foreach (object? value in values)
{
    if (value is string text)
    {
        Console.WriteLine($"string with {text.Length} characters");
    }
    else if (value is int number)
    {
        Console.WriteLine($"int, doubled: {number * 2}");
    }
    else if (value is null)
    {
        Console.WriteLine("null");
    }
    else
    {
        Console.WriteLine("something else");
    }
}
