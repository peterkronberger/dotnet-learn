List<int> numbers = [1, 2, 3];

var query = from n in numbers where n > 1 select n * 10;
Console.WriteLine("Query defined");

numbers.Add(4);
Console.WriteLine($"First run:  {string.Join(", ", query)}");

numbers.Add(5);
Console.WriteLine($"Second run: {string.Join(", ", query)}");
