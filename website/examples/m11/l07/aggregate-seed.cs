int[] numbers = [3, 5, 2];

int product = numbers.Aggregate(1, (total, n) =>
{
    Console.WriteLine($"total {total}, next {n}");
    return total * n;
});

Console.WriteLine($"Product: {product}");
Console.WriteLine($"Empty: {new int[0].Aggregate(1, (total, n) => total * n)}");
