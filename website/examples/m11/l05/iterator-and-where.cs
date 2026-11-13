// An iterator that reports when its body runs
static IEnumerable<int> Numbers()
{
    for (int i = 1; i <= 3; i++)
    {
        Console.WriteLine($"  iterator: gives {i}");
        yield return i;
    }
}

Console.WriteLine("Call the iterator");
IEnumerable<int> fromIterator = Numbers();
Console.WriteLine("Loop over the iterator");
foreach (int n in fromIterator)
{
    Console.WriteLine($"use {n}");
}

// A LINQ query whose lambda reports when it runs
List<int> source = [1, 2, 3];
Console.WriteLine("Define the query");
IEnumerable<int> query = source.Where(n =>
{
    Console.WriteLine($"  lambda: checks {n}");
    return n > 1;
});
Console.WriteLine("Loop over the query");
foreach (int n in query)
{
    Console.WriteLine($"use {n}");
}
