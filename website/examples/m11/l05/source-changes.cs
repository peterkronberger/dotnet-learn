List<int> numbers = [5, 8, 2];
int limit = 4;

// The query only describes the work
IEnumerable<int> bigQuery = numbers.Where(n => n > limit);
// ToList runs the query now and copies the result
List<int> bigList = bigQuery.ToList();
Console.WriteLine($"At first: query {bigQuery.Count()}, list {bigList.Count}");

numbers.Add(9);
Console.WriteLine($"After Add(9): query {bigQuery.Count()}, list {bigList.Count}");

limit = 7;
Console.WriteLine($"After limit = 7: query {bigQuery.Count()}, list {bigList.Count}");
