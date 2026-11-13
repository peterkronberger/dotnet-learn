int[] numbers = [4, 8, 15, 16, 23, 42];

Console.WriteLine($"First: {numbers.First()}");
Console.WriteLine($"First above 10: {numbers.First(n => n > 10)}");
Console.WriteLine($"Last: {numbers.Last()}");
Console.WriteLine($"Last below 20: {numbers.Last(n => n < 20)}");
Console.WriteLine($"Single above 40: {numbers.Single(n => n > 40)}");
Console.WriteLine($"Element at 2: {numbers.ElementAt(2)}");
