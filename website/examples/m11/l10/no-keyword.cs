int[] numbers = [5, 3, 8, 3, 1, 8];

int count = (from n in numbers where n > 2 select n).Count();
int first = (from n in numbers orderby n select n).First();
var distinct = (from n in numbers select n).Distinct().Take(3);

Console.WriteLine($"Count: {count}");
Console.WriteLine($"First: {first}");
Console.WriteLine($"Distinct: {string.Join(", ", distinct)}");
