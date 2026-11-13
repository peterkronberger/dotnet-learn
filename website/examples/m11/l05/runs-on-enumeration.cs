List<string> words = ["pear", "fig", "plum"];

Console.WriteLine("Define the query");
IEnumerable<string> longWords = words.Where(w =>
{
    Console.WriteLine($"  checks {w}");
    return w.Length > 3;
});

Console.WriteLine("First loop");
foreach (string w in longWords)
{
    Console.WriteLine($"use {w}");
}

Console.WriteLine("Second loop");
foreach (string w in longWords)
{
    Console.WriteLine($"use {w}");
}

Console.WriteLine($"Count: {longWords.Count()}");
