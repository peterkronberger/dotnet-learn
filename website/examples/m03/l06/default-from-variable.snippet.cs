int fallback = 3;
Console.WriteLine(Limit(5));

int Limit(int value, int max = fallback)
{
    return value > max ? max : value;
}
