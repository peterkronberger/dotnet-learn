Console.WriteLine(Greet("alice", "hello"));
Console.WriteLine(Greet("bob", "hello"));

string Greet(string name, string greeting)
{
    return $"{greeting}, {name}";
}
