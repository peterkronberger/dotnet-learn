int calls = 0;
List<string> words = ["pear", "fig", "plum", "kiwi"];

// A query with a counter: it shows how often the lambda runs
IEnumerable<string> fourLetters = words.Where(w =>
{
    calls++;
    return w.Length == 4;
});
Console.WriteLine($"After the definition: {calls} calls");

int count = fourLetters.Count();
Console.WriteLine($"Count() gives {count}: {calls} calls");

string[] array = fourLetters.ToArray();
Console.WriteLine($"ToArray() gives {array.Length} items: {calls} calls");

Dictionary<string, int> lengths = fourLetters.ToDictionary(w => w, w => w.Length);
Console.WriteLine($"ToDictionary() gives {lengths.Count} entries: {calls} calls");

HashSet<string> set = fourLetters.ToHashSet();
Console.WriteLine($"ToHashSet() gives {set.Count} items: {calls} calls");

// The array is a copy: using it does not call the lambda again
Console.WriteLine($"Length of the array again: {array.Length}: {calls} calls");
