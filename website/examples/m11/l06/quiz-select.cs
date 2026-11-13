int[] numbers = [1, 2, 3];
var rows = numbers.Select(n => new { n, Square = n * n });
foreach (var row in rows)
{
    Console.WriteLine($"{row.n}:{row.Square}");
}
