List<int> numbers = [5, 8, 2, 9, 4, 7];

List<int> big = numbers.Where(n => n > 4).ToList();

Console.WriteLine(big.Count);
Console.WriteLine(numbers.Count);
Console.WriteLine(string.Join(", ", numbers));
