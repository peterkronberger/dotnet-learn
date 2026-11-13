string[] words = ["pear", "fig", "apple", "kiwi", "date", "plum"];

// OrderBy sorts by the value that the key selector returns
foreach (string word in words.OrderBy(w => w.Length))
{
    Console.WriteLine($"by length: {word}");
}

// ThenBy breaks ties; the comparer fixes how strings are compared
IEnumerable<string> sorted = words
    .OrderBy(w => w.Length)
    .ThenBy(w => w, StringComparer.Ordinal);
Console.WriteLine(string.Join(", ", sorted));

// OrderByDescending sorts from the biggest key to the smallest
Console.WriteLine(string.Join(", ", words.OrderByDescending(w => w.Length)));
