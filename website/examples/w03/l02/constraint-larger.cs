static T Larger<T>(T a, T b) where T : IComparable<T>
{
    if (a.CompareTo(b) >= 0)
    {
        return a;
    }
    return b;
}

Console.WriteLine(Larger(3, 9));
Console.WriteLine(Larger(-4, -8));
