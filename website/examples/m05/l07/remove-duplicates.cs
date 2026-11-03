string[] events = { "logout", "login-failed", "logout", "logout" };
HashSet<string> kinds = new HashSet<string>(events);

Console.WriteLine(events.Length);
Console.WriteLine(kinds.Count);
Console.WriteLine(kinds.Contains("logout"));
Console.WriteLine(kinds.Contains("login-success"));
