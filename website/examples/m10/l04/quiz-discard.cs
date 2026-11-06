Func<int, int, int> pick = (_, _) => 7;
Func<int, int, int> second = (_, b) => b;

Console.WriteLine(pick(1, 2));
Console.WriteLine(second(1, 2));
