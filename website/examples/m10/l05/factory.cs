static Func<int, bool> AtLeast(int threshold) =>
    value => value >= threshold;

Func<int, bool> atLeast2 = AtLeast(2);
Func<int, bool> atLeast5 = AtLeast(5);

Console.WriteLine(atLeast2(3));
Console.WriteLine(atLeast5(3));
Console.WriteLine(atLeast5(7));
