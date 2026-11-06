int limit = 3;
Func<int, bool> over = number => number > limit;
Console.WriteLine(over(5));

limit = 10;
Console.WriteLine(over(5));

int total = 0;
Action<int> add = number => total += number;
add(2);
add(5);
Console.WriteLine(total);
