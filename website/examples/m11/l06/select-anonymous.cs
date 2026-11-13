List<string> words = ["pear", "fig", "plum", "kiwi"];

// Select gives two values per element
var rows = words
    .Where(w => w.Length == 4)
    .Select(w => new { Word = w, w.Length, Upper = w.ToUpper() });

foreach (var row in rows)
{
    Console.WriteLine($"{row.Word} {row.Length} {row.Upper}");
}

// Without a name for the type, a row is printed with ToString()
foreach (var row in rows)
{
    Console.WriteLine(row);
}
