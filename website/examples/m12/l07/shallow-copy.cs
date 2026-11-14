// The copy shares the list of the original
Watchlist first = new("night shift", ["bob"]);
Watchlist second = first with { Name = "day shift" };

first.Users.Add("dave");

Console.WriteLine(first.Users.Count);
Console.WriteLine(second.Users.Count);
Console.WriteLine(ReferenceEquals(first.Users, second.Users));

record Watchlist(string Name, List<string> Users);
