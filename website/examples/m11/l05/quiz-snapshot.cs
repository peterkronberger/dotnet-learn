List<int> numbers = [1, 2, 3];
IEnumerable<int> query = numbers.Where(n => n > 1);
List<int> copy = query.ToList();
numbers.Add(4);
Console.WriteLine($"{query.Count()} {copy.Count}");
