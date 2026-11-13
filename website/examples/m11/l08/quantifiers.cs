int[] numbers = [4, 8, 15, 16];
int[] empty = [];

Console.WriteLine($"Any: {numbers.Any()}");
Console.WriteLine($"Any above 10: {numbers.Any(n => n > 10)}");
Console.WriteLine($"All above 3: {numbers.All(n => n > 3)}");
Console.WriteLine($"All above 10: {numbers.All(n => n > 10)}");
Console.WriteLine($"Contains 15: {numbers.Contains(15)}");
Console.WriteLine($"Empty Any: {empty.Any()}");
Console.WriteLine($"Empty All: {empty.All(n => n > 10)}");
Console.WriteLine($"Empty Contains: {empty.Contains(1)}");
