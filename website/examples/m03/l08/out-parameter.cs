bool isEven = TryHalf(8, out int half);
Console.WriteLine($"{isEven} {half}");
isEven = TryHalf(7, out half);
Console.WriteLine($"{isEven} {half}");
Console.WriteLine(TryHalf(10, out var other));
Console.WriteLine(other);

bool TryHalf(int number, out int half)
{
    if (number % 2 == 0)
    {
        half = number / 2;
        return true;
    }
    half = 0;
    return false;
}
