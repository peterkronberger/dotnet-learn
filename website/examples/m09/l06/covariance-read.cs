List<string> names = ["alice", "bob"];

IEnumerable<object> things = names;
IReadOnlyList<object> readOnly = names;

PrintAll(things);
Console.WriteLine(readOnly.Count);
Console.WriteLine(readOnly[1]);

static void PrintAll(IEnumerable<object> items)
{
    foreach (object item in items)
    {
        Console.WriteLine($"Item: {item}");
    }
}
