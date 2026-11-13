List<string> users = [with(capacity: 10), "alice", "bob"];

Console.WriteLine($"Count: {users.Count}");
Console.WriteLine($"Capacity: {users.Capacity}");
