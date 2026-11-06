Console.WriteLine(Max(3, 8));
Console.WriteLine(Max("pear", "apple"));
Console.WriteLine(Max(2.5, 1.5));

static T Max<T>(T a, T b) where T : IComparable<T>
{
    if (a.CompareTo(b) >= 0)
    {
        return a;
    }
    return b;
}
