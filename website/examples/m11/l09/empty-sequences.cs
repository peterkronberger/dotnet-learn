int[] numbers = [3, 1, 3];
int[] none = [];

var joined = numbers.Join(none, n => n, m => m, (n, m) => n);
var grouped = numbers.GroupJoin(none, n => n, m => m, (n, g) => g.Count());

Console.WriteLine($"Join: {joined.Count()} pairs");
Console.WriteLine($"GroupJoin: {string.Join(", ", grouped)}");
Console.WriteLine($"Except: {string.Join(", ", numbers.Except(none))}");
Console.WriteLine($"Intersect: {numbers.Intersect(none).Count()} elements");
Console.WriteLine($"Union: {string.Join(", ", numbers.Union(none))}");
