// Split an object with its Deconstruct method
Login login = new("bob", "203.0.113.7");
var (user, ip) = login;
Console.WriteLine($"{user} from {ip}");
var (onlyUser, _) = login;
Console.WriteLine(onlyUser);

class Login
{
    public string User { get; }
    public string Ip { get; }

    public Login(string user, string ip)
    {
        User = user;
        Ip = ip;
    }

    public void Deconstruct(out string user, out string ip)
    {
        user = User;
        ip = Ip;
    }
}
