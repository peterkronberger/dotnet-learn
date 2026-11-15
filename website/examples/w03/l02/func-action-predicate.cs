List<int> sizes = [120, 40, 300, 75];

Predicate<int> isLarge = size => size >= 100;
Func<int, string> label = size => $"{size} KB";
Action<string> show = text => Console.WriteLine(text);

foreach (int size in sizes)
{
    if (isLarge(size))
    {
        show(label(size));
    }
}
