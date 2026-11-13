// Gives the first count even numbers from the start value on
static IEnumerable<int> EvenNumbers(int start, int count)
{
    int given = 0;
    for (int n = start; ; n++)
    {
        if (given == count)
        {
            yield break;
        }
        if (n % 2 == 0)
        {
            yield return n;
            given++;
        }
    }
}

Console.WriteLine(string.Join(", ", EvenNumbers(3, 4)));
