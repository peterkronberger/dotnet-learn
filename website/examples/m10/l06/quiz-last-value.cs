Func<int, int> scale = number => number * 2;
scale += number => number + 100;
scale += number => number - 1;

Console.WriteLine(scale(10));
