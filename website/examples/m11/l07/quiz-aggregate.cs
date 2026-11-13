int[] numbers = [2, 3, 4];

int result = numbers.Aggregate(10, (total, n) => total - n);

Console.WriteLine(result);
