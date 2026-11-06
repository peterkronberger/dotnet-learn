int small = Pick(true, 10, 20);
Console.WriteLine(small);

string word = Pick(false, "ten", "twenty");
Console.WriteLine(word);

var mixed = Pick(true, 1, 2.5);
Console.WriteLine(mixed);

static T Pick<T>(bool takeFirst, T a, T b)
{
    Console.WriteLine($"T is {typeof(T).Name}");
    if (takeFirst)
    {
        return a;
    }
    return b;
}
