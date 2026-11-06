int calls = 0;
Func<int, int> next = number =>
{
    calls++;
    return number + 1;
};

Console.WriteLine($"Before: {calls}");
next(1);
next(2);
Console.WriteLine($"After: {calls}");
