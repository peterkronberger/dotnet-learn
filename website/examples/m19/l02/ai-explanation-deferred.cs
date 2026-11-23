var numbers = new List<int> { 1, 2, 3 };
var even = numbers.Where(n => n % 2 == 0);

numbers.Add(4);

Console.WriteLine(even.Count());
