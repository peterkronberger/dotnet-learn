string[] users = ["alice", "bob", "carol"];

bool anyShort = users.Any(u => u.Length < 4);
bool allLower = users.All(u => u == u.ToLower());
string? firstLong = users.FirstOrDefault(u => u.Length > 4);

Console.WriteLine($"Any short: {anyShort}");
Console.WriteLine($"All lower: {allLower}");
Console.WriteLine($"First long: {firstLong}");
