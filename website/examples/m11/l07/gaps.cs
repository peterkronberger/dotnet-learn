string[] words = ["fig", "plum", "pear", "kiwi", "apple", "grape"];

var groups = words
    .GroupBy(w => w.Length)
    .OrderBy(g => g.Key);

foreach (var lengthGroup in groups)
{
    Console.WriteLine($"{lengthGroup.Key} letters: {lengthGroup.Count()} words");
}
