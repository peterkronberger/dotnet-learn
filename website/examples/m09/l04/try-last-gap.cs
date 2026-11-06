List<int> numbers = [4, 8];

if (TryFirst(numbers, out int first))
{
    Console.WriteLine($"First: {first}");
}
// Task 1: call TryFirst with an empty List<string> and print the result
// Task 2: write TryLast<T> and call it with numbers

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
