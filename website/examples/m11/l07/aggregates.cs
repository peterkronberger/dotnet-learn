int[] numbers = [4, 8, 15, 16, 23, 42];

Console.WriteLine($"Count: {numbers.Count()}");
Console.WriteLine($"Count above 10: {numbers.Count(n => n > 10)}");
Console.WriteLine($"Sum: {numbers.Sum()}");
Console.WriteLine($"Min: {numbers.Min()}");
Console.WriteLine($"Max: {numbers.Max()}");
Console.WriteLine($"Average: {numbers.Average()}");

string[] words = ["pear", "fig", "banana", "plum"];

Console.WriteLine($"Shortest: {words.MinBy(w => w.Length)}");
Console.WriteLine($"Longest: {words.MaxBy(w => w.Length)}");
