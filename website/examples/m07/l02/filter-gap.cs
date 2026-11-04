// Replace the if statement in the catch block by an exception filter:
// two catch clauses for FormatException, the first with a when filter.
// The output stays the same.

string[] inputs = ["12", "", "abc"];
foreach (string text in inputs)
{
    try
    {
        Console.WriteLine(int.Parse(text));
    }
    catch (FormatException)
    {
        if (text.Length == 0)
        {
            Console.WriteLine("Empty text");
        }
        else
        {
            Console.WriteLine($"Not a number: {text}");
        }
    }
}
