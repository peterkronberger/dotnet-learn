string line = "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7";

string[] parts = line.Split(' ');
Console.WriteLine(parts.Length);
Console.WriteLine(parts[1]);
Console.WriteLine(string.Join(" | ", parts));
