Console.WriteLine("Start");
SayHello("alice");
Console.WriteLine("Between");
SayHello("bob");
SayHello("carol");
Console.WriteLine("End");

void SayHello(string name)
{
    Console.WriteLine($"Hello {name}");
}
