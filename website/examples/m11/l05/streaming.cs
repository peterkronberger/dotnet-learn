int[] numbers = [3, 1, 2];

// Where is streaming: it checks one element, then gives it away
Console.WriteLine("Where:");
IEnumerable<int> filtered = numbers.Where(n =>
{
    Console.WriteLine($"  checks {n}");
    return n > 0;
});
foreach (int n in filtered)
{
    Console.WriteLine($"use {n}");
}

// OrderBy is non-streaming: it reads the whole source before the first element
Console.WriteLine("OrderBy:");
IEnumerable<int> sorted = numbers.OrderBy(n =>
{
    Console.WriteLine($"  key of {n}");
    return n;
});
foreach (int n in sorted)
{
    Console.WriteLine($"use {n}");
}
