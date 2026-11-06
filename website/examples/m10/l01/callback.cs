int[] numbers = [3, 8, 5, 10];
Console.WriteLine(CountMatching(numbers, IsEven));
Console.WriteLine(CountMatching(numbers, IsBig));

static int CountMatching(int[] values, NumberCheck check)
{
    int count = 0;
    foreach (int value in values)
    {
        if (check(value))
        {
            count++;
        }
    }
    return count;
}

static bool IsEven(int number)
{
    return number % 2 == 0;
}

static bool IsBig(int number)
{
    return number >= 5;
}

delegate bool NumberCheck(int number);
