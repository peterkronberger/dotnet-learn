List<int> numbers = [5, 8, 2, 9, 4, 7];

// Where keeps the elements for which the lambda returns true
IEnumerable<int> big = numbers.Where(n => n > 4);
foreach (int n in big)
{
    Console.WriteLine($"big: {n}");
}

// Select turns every element into a new value
IEnumerable<int> doubled = numbers.Select(n => n * 2);
Console.WriteLine(string.Join(", ", doubled));

// The static form is the same call as the extension method
IEnumerable<int> same = Enumerable.Where(numbers, n => n > 4);
Console.WriteLine(string.Join(", ", same));
