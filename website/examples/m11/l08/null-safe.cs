string[] words = ["pear", "fig", "banana"];

string? longWord = words.FirstOrDefault(w => w.Length > 10);
Console.WriteLine($"Length: {longWord?.Length}");
Console.WriteLine($"Text: {longWord ?? "no long word"}");
Console.WriteLine($"Upper: {words.FirstOrDefault(w => w.Length == 3)?.ToUpper()}");
