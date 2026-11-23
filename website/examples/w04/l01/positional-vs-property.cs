Login login = new("alice", "192.0.2.10");
Console.WriteLine(login is ("192.0.2.10", "alice"));
Console.WriteLine(login is { Ip: "192.0.2.10", User: "alice" });
Console.WriteLine(login is ("alice", _));

class Login(string user, string ip)
{
    public string User { get; } = user;
    public string Ip { get; } = ip;

    public void Deconstruct(out string user, out string ip)
    {
        user = User;
        ip = Ip;
    }
}
