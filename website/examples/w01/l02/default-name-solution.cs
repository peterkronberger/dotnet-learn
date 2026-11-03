string? line = Console.ReadLine();
string name = line ?? "unknown";
Console.WriteLine($"Name: {name}");
