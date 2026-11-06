object first = Max(new object(), new object());
int second = Max(1, 2);

static T Max<T>(T a, T b) where T : IComparable<T>
{
    return a.CompareTo(b) >= 0 ? a : b;
}
