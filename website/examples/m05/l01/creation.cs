// new T[n] creates n elements; each gets the default value of its type
int[] counts = new int[3];
bool[] seen = new bool[2];
string?[] names = new string?[2];
Console.WriteLine(counts[0]);
Console.WriteLine(seen[1]);
Console.WriteLine(names[0] == null);
// An array initializer lists the values at once
var ips = new[] { "192.0.2.10", "198.51.100.8", "203.0.113.7" };
string[] users = { "alice", "bob" };
Console.WriteLine(ips.Length);
Console.WriteLine(users.Length);
