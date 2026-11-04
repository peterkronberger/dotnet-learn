// Rewrite the check with int.Parse, try and catch instead of TryParse.
// The output stays the same: Not a number: 4x2, then Done.

string text = "4x2";
if (int.TryParse(text, out int number))
{
    Console.WriteLine($"Number: {number}");
}
else
{
    Console.WriteLine($"Not a number: {text}");
}
Console.WriteLine("Done");
