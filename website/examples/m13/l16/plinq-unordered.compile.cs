// Without AsOrdered the order of the results is not fixed.
int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8];

foreach (int square in numbers.AsParallel().Select(n => n * n))
{
    Console.WriteLine(square);
}
