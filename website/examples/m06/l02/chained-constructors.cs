Counter first = new Counter("logins", 3);
Console.WriteLine(first.Describe());

Counter second = new Counter("logouts");
Console.WriteLine(second.Describe());

Counter third = new Counter();
Console.WriteLine(third.Describe());

class Counter
{
    public string Name;
    public int Value;

    public Counter(string name, int start)
    {
        Console.WriteLine($"two parameters: {name}, {start}");
        Name = name;
        Value = start;
    }

    public Counter(string name) : this(name, 0)
    {
        Console.WriteLine("one parameter done");
    }

    public Counter() : this("unnamed")
    {
        Console.WriteLine("no parameters done");
    }

    public string Describe()
    {
        return $"[{Name}] {Value}";
    }
}
