int[] numbers = [1, 5, 9];
int limit = 4;
IEnumerable<int> big = numbers.Where(n => n > limit);
limit = 8;
Console.WriteLine(big.Count());
