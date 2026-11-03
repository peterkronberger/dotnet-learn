string line = "login-failed user=bob";
string[] parts = line.Split(' ');
string user = parts[1].Replace("user=", "");

Console.WriteLine(parts.Length);
Console.WriteLine(user);
Console.WriteLine(parts[1]);
