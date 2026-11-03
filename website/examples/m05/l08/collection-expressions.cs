int[] numbers = [10, 20, 30];
List<int> sameNumbers = [10, 20, 30];
List<int> none = [];

Console.WriteLine(numbers.Length);
Console.WriteLine(sameNumbers.Count);
Console.WriteLine(none.Count);
sameNumbers.Add(40);
Console.WriteLine(sameNumbers.Count);
