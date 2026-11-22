// Task 1: try to parse the text with int.TryParse
// Task 2: return null if the text is not a number
// Task 3: print "unreadable" for null with the ?? operator
Console.WriteLine(ReadFailures("5")?.ToString() ?? "");
Console.WriteLine(ReadFailures("x7")?.ToString() ?? "");

static int? ReadFailures(string text)
{
    return 0;
}
