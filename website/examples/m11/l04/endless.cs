// An endless sequence: 1, 2, 4, 8, ...
static IEnumerable<int> Doubling()
{
    int value = 1;
    while (true)
    {
        yield return value;
        value *= 2;
    }
}

// Take asks for 6 elements only, so only 6 are computed
Console.WriteLine(string.Join(", ", Doubling().Take(6)));

// Where and Take work together on the same sequence
Console.WriteLine(string.Join(", ", Doubling().Where(v => v > 50).Take(2)));
