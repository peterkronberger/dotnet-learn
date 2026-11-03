Console.WriteLine(TryHalf(8, out int result));
Console.WriteLine(result);
Console.WriteLine(TryHalf(7, out int other));
Console.WriteLine(other);

static bool TryHalf(int n, out int half)
{
    half = 0;
    if (n % 2 != 0)
    {
        return false;
    }
    half = n / 2;
    return true;
}
