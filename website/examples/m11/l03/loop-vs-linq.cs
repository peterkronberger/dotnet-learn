List<string> users = ["carol", "al", "bob", "alice", "dave", "ed"];

// With a loop: filter, then sort
List<string> longNamesLoop = [];
foreach (string name in users)
{
    if (name.Length > 3)
    {
        longNamesLoop.Add(name);
    }
}
longNamesLoop.Sort(StringComparer.Ordinal);
Console.WriteLine(string.Join(", ", longNamesLoop));

// With a LINQ chain: the same result
List<string> longNamesLinq = users
    .Where(name => name.Length > 3)
    .OrderBy(name => name, StringComparer.Ordinal)
    .ToList();
Console.WriteLine(string.Join(", ", longNamesLinq));
