// Two patterns that look alike but group differently
string[] types = ["login-success", "login-failed", "logout"];
foreach (string type in types)
{
    bool first = type is not "login-success" or "logout";
    bool second = type is not ("login-success" or "logout");
    Console.WriteLine($"{type}: {first} {second}");
}
