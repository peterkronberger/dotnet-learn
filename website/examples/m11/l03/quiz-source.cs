List<string> names = ["bob", "alice", "eve"];

List<string> shortNames = names.Where(n => n.Length <= 3).ToList();

Console.WriteLine(shortNames.Count);
Console.WriteLine(names.Count);
