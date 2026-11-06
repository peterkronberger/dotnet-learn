List<int> numbers = [4, 8];
List<string> empty = [];

if (TryFirst(numbers, out int number))
{
    Console.WriteLine($"First number: {number}");
}
if (!TryFirst(empty, out string? word))
{
    Console.WriteLine($"No word, value is null: {word is null}");
}

static bool TryFirst<T>(List<T> items, out T? value)
{
    if (items.Count == 0)
    {
        value = default;
        return false;
    }
    value = items[0];
    return true;
}
