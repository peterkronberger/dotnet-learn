// Goal: the counter starts at 3, so the output is "[logins] 3".
Counter counter = new Counter("logins", 3);
Console.WriteLine(counter.Describe());

class Counter
{
    public string Name;
    public int Value;

    public Counter(string name, int start)
    {
        Name = name;
        Value = 0;
    }

    public string Describe()
    {
        return $"[{Name}] {Value}";
    }
}
