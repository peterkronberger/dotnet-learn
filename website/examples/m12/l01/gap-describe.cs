// Describe a value with a switch expression and type patterns
static string Describe(object? value) => value switch
{
    null => "nothing",
    string text => $"text of length {text.Length}",
    int number => $"number {number}",
    _ => "other",
};

Console.WriteLine(Describe("log"));
Console.WriteLine(Describe(7));
Console.WriteLine(Describe(null));
Console.WriteLine(Describe(2.5));
