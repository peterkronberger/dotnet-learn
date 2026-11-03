HashSet<string> users = new HashSet<string>();
Console.WriteLine(users.Add("bob"));
Console.WriteLine(users.Add("alice"));
Console.WriteLine(users.Add("bob"));
Console.WriteLine(users.Count);
Console.WriteLine(users.Contains("alice"));
Console.WriteLine(users.Remove("alice"));
Console.WriteLine(users.Contains("alice"));
