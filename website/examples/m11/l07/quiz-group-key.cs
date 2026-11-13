int[] numbers = [1, 2, 3, 4, 5];

foreach (var part in numbers.GroupBy(n => n % 2))
{
    Console.WriteLine($"{part.Key}: {part.Sum()}");
}
