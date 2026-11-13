string[] words = ["apple", "banana", "avocado", "blueberry", "cherry", "apricot"];

IEnumerable<IGrouping<char, string>> groups = words.GroupBy(w => w[0]);

foreach (IGrouping<char, string> letterGroup in groups)
{
    Console.WriteLine($"{letterGroup.Key}: {string.Join(", ", letterGroup)}");
}
