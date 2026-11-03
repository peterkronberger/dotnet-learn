string text = "42";

if (int.TryParse(text, out int number))
{
    Console.WriteLine($"Number: {number}");
}
else
{
    Console.WriteLine("Not a number");
}
