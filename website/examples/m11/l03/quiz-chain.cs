int[] numbers = [3, 8, 1, 6, 4];

IEnumerable<int> result = numbers
    .Where(n => n > 2)
    .OrderByDescending(n => n)
    .Select(n => n * 10)
    .Take(2);

Console.WriteLine(string.Join(", ", result));
