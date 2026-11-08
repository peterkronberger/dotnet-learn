Func<int> next = MakeCounter();
Console.WriteLine(next());
Console.WriteLine(next());
Console.WriteLine(next());

static Func<int> MakeCounter()
{
    int counter = 0;
    return () =>
    {
        counter++;
        return counter;
    };
}
