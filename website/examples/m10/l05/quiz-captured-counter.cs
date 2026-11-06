int calls = 0;
Func<int, int> next = number =>
{
    calls++;
    return number + calls;
};

Console.WriteLine(next(10));
Console.WriteLine(next(10));
Console.WriteLine(calls);
