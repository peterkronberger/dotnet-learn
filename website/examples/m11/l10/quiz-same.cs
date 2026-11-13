int[] numbers = [1, 2, 3, 4];

var query = from n in numbers where n % 2 == 0 select n;
var methods = numbers.Where(n => n % 2 == 0);

Console.WriteLine(query.SequenceEqual(methods));
