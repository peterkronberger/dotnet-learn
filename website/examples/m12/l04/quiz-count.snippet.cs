Visit visit = new("bob", "203.0.113.7");
var (user, ip) = visit;
(string name, string address, string port) = visit;
Console.WriteLine($"{user} {ip}");

record Visit(string User, string Ip);
