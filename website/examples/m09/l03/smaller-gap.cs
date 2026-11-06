Console.WriteLine(Smaller(4, 9));
// Task 1: call Smaller with the two strings "pear" and "apple" and print it
// Task 2: write Larger<T> with the same constraint and call it with 2.5 and 1.5

static T Smaller<T>(T a, T b) where T : IComparable<T>
{
    if (a.CompareTo(b) <= 0)
    {
        return a;
    }
    return b;
}
