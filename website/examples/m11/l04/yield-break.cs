// Gives the numbers up to a limit, and stops at the first negative number
static IEnumerable<int> UntilNegative(int[] numbers, int limit)
{
    foreach (int n in numbers)
    {
        if (n < 0)
        {
            yield break;
        }
        if (n > limit)
        {
            continue;
        }
        yield return n;
    }
}

int[] data = [3, 12, 5, -1, 4];
Console.WriteLine(string.Join(", ", UntilNegative(data, 10)));

// Every foreach starts a new run of the body
IEnumerable<int> run = UntilNegative(data, 10);
Console.WriteLine(string.Join(", ", run));
Console.WriteLine(string.Join(", ", run));
