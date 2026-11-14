// Two patterns that look alike but group differently
string[] types = ["login-success", "login-failed", "logout"];
foreach (string type in types)
{
    // The compiler warns: "logout" is redundant (the lesson shows this)
#pragma warning disable CS9336
    bool first = type is not "login-success" or "logout";
#pragma warning restore CS9336
    bool second = type is not ("login-success" or "logout");
    Console.WriteLine($"{type}: {first} {second}");
}
