string[] words = ["apple", "banana", "avocado", "blueberry", "cherry", "apricot"];

var rows = words
    .GroupBy(w => w[0])
    .Select(g => new { Letter = g.Key, Count = g.Count() });

foreach (var row in rows)
{
    Console.WriteLine(row);
}

Dictionary<char, int> counts = words
    .GroupBy(w => w[0])
    .ToDictionary(g => g.Key, g => g.Count());
Console.WriteLine(counts['a']);
