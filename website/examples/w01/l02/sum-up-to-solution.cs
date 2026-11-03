Console.WriteLine($"Sum: {SumUpTo(4)}");

static int SumUpTo(int n)
{
    if (n == 0)
    {
        return 0;
    }
    return n + SumUpTo(n - 1);
}
