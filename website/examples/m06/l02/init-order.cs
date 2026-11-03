Counter counter = new Counter("logins", 3);
Console.WriteLine(counter.Describe());

class Counter
{
    public string Name;
    public int Value = 10;

    public Counter(string name, int start)
    {
        Console.WriteLine($"Start of constructor: Value is {Value}");
        this.Name = name;
        this.Value = start;
        Console.WriteLine("End of constructor");
    }

    public string Describe()
    {
        return $"[{Name}] {Value}";
    }
}
