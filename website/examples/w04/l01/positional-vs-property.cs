Login login = new("alice", "192.0.2.10");
Console.WriteLine(login is ("192.0.2.10", "alice"));
Console.WriteLine(login is { Ip: "192.0.2.10", User: "alice" });
Console.WriteLine(login is ("alice", _));

record Login(string User, string Ip);
