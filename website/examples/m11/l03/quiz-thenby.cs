string[] words = ["bb", "a", "ccc", "aa", "b"];

IEnumerable<string> sorted = words
    .OrderBy(w => w.Length)
    .ThenBy(w => w, StringComparer.Ordinal);

foreach (string w in sorted)
{
    Console.WriteLine(w);
}
