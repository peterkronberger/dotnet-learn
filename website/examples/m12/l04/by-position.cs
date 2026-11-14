// A positional pattern compares by position, not by name
Visit visit = new("bob", "203.0.113.7");
Console.WriteLine(visit is ("bob", "203.0.113.7"));
Console.WriteLine(visit is ("203.0.113.7", "bob"));
Console.WriteLine(visit is (User: "bob", Ip: "203.0.113.7"));
Console.WriteLine(visit is (_, _));

record Visit(string User, string Ip);
