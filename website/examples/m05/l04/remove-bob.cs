List<string> users = new List<string>();
users.Add("alice");
users.Add("bob");
users.Add("carol");
users.RemoveAt(2);
Console.WriteLine(users.Count);
Console.WriteLine(users[1]);
