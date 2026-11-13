int[] numbers = [4, 8, 15];
string[] words = ["pear", "fig"];

Console.WriteLine($"int: {numbers.FirstOrDefault(n => n > 100)}");
Console.WriteLine($"string is null: {words.FirstOrDefault(w => w.Length > 10) is null}");
Console.WriteLine($"own default: {numbers.FirstOrDefault(n => n > 100, -1)}");
Console.WriteLine($"Single: {numbers.SingleOrDefault(n => n > 100)}");
Console.WriteLine($"Last: {numbers.LastOrDefault(n => n > 100)}");
Console.WriteLine($"ElementAt: {numbers.ElementAtOrDefault(10)}");
