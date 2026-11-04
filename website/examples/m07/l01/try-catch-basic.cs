Console.WriteLine("Before the try block");
try
{
    int number = int.Parse("seven");
    Console.WriteLine($"Number: {number}");
}
catch (FormatException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}
Console.WriteLine("After the try statement");
