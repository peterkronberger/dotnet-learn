int[] numbers = [3, 8, 1, 6];

var query = from n in numbers where n > 2 orderby n descending select n + 1;
Console.WriteLine(string.Join(", ", query));
