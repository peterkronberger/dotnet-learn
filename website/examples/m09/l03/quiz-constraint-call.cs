Console.WriteLine(Pick("b", "a"));
Console.WriteLine(Pick(3, 7));
Console.WriteLine(Pick(2.5, 2.5));

static T Pick<T>(T a, T b) where T : IComparable<T>
{
    return a.CompareTo(b) > 0 ? a : b;
}
