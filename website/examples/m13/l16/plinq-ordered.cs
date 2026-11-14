// PLINQ with AsOrdered: the results keep the order of the source.
int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8];

int[] squares = numbers
    .AsParallel()
    .AsOrdered()
    .Select(n => n * n)
    .ToArray();

Console.WriteLine(string.Join(", ", squares));
Console.WriteLine($"Even numbers: {numbers.AsParallel().Count(n => n % 2 == 0)}");
