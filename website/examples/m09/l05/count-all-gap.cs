int[] numbers = [5, 6, 7];
Console.WriteLine($"Numbers: {CountAll(numbers)}");
// Task 1: create a Dictionary<string, int> with two entries
// and print how many entries CountAll finds
// Task 2: write LastOf<T>(IReadOnlyList<T> items)
// and print the last of the numbers

static int CountAll<T>(IEnumerable<T> items)
{
    int count = 0;
    foreach (T item in items)
    {
        count++;
    }
    return count;
}
