Login[] first = [new Login("bob", 8), new Login("bob", 8), new Login("amy", 9)];
Login[] second = [new Login("amy", 9), new Login("dave", 9)];

Console.WriteLine($"Records: {first.Union(second).Count()}");

string[] names = ["Bob", "bob", "AMY"];
Console.WriteLine($"Default: {names.Distinct().Count()}");
Console.WriteLine(
    $"Ignore case: {names.Distinct(StringComparer.OrdinalIgnoreCase).Count()}");

var byUser = first.DistinctBy(login => login.User);
Console.WriteLine($"DistinctBy: {string.Join(", ", byUser.Select(l => l.User))}");

record Login(string User, int Hour);
