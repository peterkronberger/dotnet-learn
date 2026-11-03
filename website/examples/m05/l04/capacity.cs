List<string> users = new List<string>();
users.Add("alice");
users.Add("bob");
Console.WriteLine(users.Count);
Console.WriteLine(users.Capacity >= users.Count);
