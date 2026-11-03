string? user = null;
string? ip = "192.0.2.10";
Console.WriteLine(user?.Length == null);
Console.WriteLine(user ?? "none");
user ??= "bob";
user ??= "alice";
Console.WriteLine(user);
Console.WriteLine(ip?.Length);
