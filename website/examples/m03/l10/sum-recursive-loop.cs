Console.WriteLine(SumRecursive(4));
Console.WriteLine(SumLoop(4));

int SumRecursive(int n)
{
    if (n == 0)
    {
        return 0;
    }
    return n + SumRecursive(n - 1);
}

int SumLoop(int n)
{
    int total = 0;
    for (int i = 1; i <= n; i++)
    {
        total += i;
    }
    return total;
}
