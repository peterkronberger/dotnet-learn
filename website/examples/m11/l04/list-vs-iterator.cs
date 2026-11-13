// Builds the whole list first, then returns it
static List<int> SquaresList(int count)
{
    List<int> result = [];
    for (int i = 1; i <= count; i++)
    {
        Console.WriteLine($"  list: compute {i}");
        result.Add(i * i);
    }
    return result;
}

// Gives the elements one by one
static IEnumerable<int> SquaresIterator(int count)
{
    for (int i = 1; i <= count; i++)
    {
        Console.WriteLine($"  iterator: compute {i}");
        yield return i * i;
    }
}

Console.WriteLine("List:");
foreach (int square in SquaresList(3))
{
    Console.WriteLine($"use {square}");
}

Console.WriteLine("Iterator:");
foreach (int square in SquaresIterator(3))
{
    Console.WriteLine($"use {square}");
}
