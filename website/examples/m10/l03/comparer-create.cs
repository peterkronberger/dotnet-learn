List<string> words = ["pear", "fig", "banana", "apple"];

// Sort with a Comparison<string> lambda: shortest word first
words.Sort((a, b) => a.Length.CompareTo(b.Length));
Console.WriteLine(string.Join(", ", words));

// Comparer<T>.Create turns the lambda into an IComparer<string>
IComparer<string> reverse = Comparer<string>.Create(
    (a, b) => string.CompareOrdinal(b, a));
words.Sort(reverse);
Console.WriteLine(string.Join(", ", words));

// RemoveAll and ForEach take lambdas too
int removed = words.RemoveAll(word => word.Length < 4);
Console.WriteLine($"Removed {removed}");
words.ForEach(word => Console.WriteLine($"- {word}"));
