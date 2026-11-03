HashSet<int> numbers = new HashSet<int>();
numbers.Add(3);
numbers.Add(5);
bool first = numbers.Add(3);
numbers.Remove(5);
numbers.Add(7);

Console.WriteLine(first);
Console.WriteLine(numbers.Count);
Console.WriteLine(numbers.Contains(5));
