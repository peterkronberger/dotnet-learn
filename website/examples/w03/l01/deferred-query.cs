List<int> failures = [1, 4, 6];

var many = failures.Where(n =>
{
    Console.WriteLine($"check {n}");
    return n >= 4;
});
Console.WriteLine("defined");

failures.Add(9);
Console.WriteLine(many.Count());

List<int> snapshot = many.ToList();
failures.Add(12);
Console.WriteLine(snapshot.Count);
