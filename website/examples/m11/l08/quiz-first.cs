int[] numbers = [3, 6, 9];

Console.WriteLine(numbers.FirstOrDefault(n => n > 10));
Console.WriteLine(numbers.FirstOrDefault(n => n > 10, 99));
