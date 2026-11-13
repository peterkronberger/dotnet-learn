// Task 1: make IsLong an extension method of string (the this modifier)
// Task 2: return true if the text has more than limit characters
// Task 3: call it on the text like an instance method
string text = "extension";
Console.WriteLine(TextExtensions.IsLong(text, 5));

static class TextExtensions
{
    public static bool IsLong(string text, int limit)
    {
        return false;
    }
}
