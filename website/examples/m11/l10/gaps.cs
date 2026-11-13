int[] numbers = [4, 9, 2, 7, 5];

var query =
    from n in numbers
    where n > 3
    orderby n
    select n * n;

var methods = numbers
    .Where(n => n > 3)
    .OrderBy(n => n)
    .Select(n => n * n);

Console.WriteLine(string.Join(", ", query));
Console.WriteLine(string.Join(", ", methods));
