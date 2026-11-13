List<int> numbers = [10, 20, 30, 40, 50, 60, 70];

// Take keeps the first elements, Skip drops them
Console.WriteLine(string.Join(", ", numbers.Take(3)));
Console.WriteLine(string.Join(", ", numbers.Skip(5)));

// Skip and Take together give one page of two elements
Console.WriteLine(string.Join(", ", numbers.Skip(2).Take(2)));

// ToList and ToArray turn a sequence into a collection
List<int> list = numbers.Where(n => n >= 40).ToList();
int[] array = numbers.Where(n => n < 40).ToArray();
Console.WriteLine($"{list.Count} in the list, {array.Length} in the array");

// Select can also receive the index of the element
foreach (string line in numbers.Take(2).Select((n, i) => $"{i}: {n}"))
{
    Console.WriteLine(line);
}
