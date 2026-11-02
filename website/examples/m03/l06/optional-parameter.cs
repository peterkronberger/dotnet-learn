Console.WriteLine(Greet("alice"));
Console.WriteLine(Greet("bob", "welcome"));

string Greet(string name, string greeting = "hello")
{
    return $"{greeting}, {name}";
}
