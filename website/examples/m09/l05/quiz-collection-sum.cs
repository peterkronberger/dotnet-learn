List<string> words = ["a", "bb", "ccc"];
string[] more = ["dddd"];

Console.WriteLine(TotalLength(words));
Console.WriteLine(TotalLength(more));

static int TotalLength(IEnumerable<string> items)
{
    int total = 0;
    foreach (string item in items)
    {
        total += item.Length;
    }
    return total;
}
