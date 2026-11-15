int[] failures = [3, 5, 2];
string[] users = ["ana", "ben"];

Console.WriteLine(failures.FirstOrDefault(n => n > 10));
Console.WriteLine(users.FirstOrDefault(u => u.StartsWith('x')) is null);

try
{
    Console.WriteLine(failures.First(n => n > 10));
}
catch (InvalidOperationException)
{
    Console.WriteLine("no match");
}

try
{
    Console.WriteLine(failures.Single(n => n > 2));
}
catch (InvalidOperationException)
{
    Console.WriteLine("not exactly one");
}
