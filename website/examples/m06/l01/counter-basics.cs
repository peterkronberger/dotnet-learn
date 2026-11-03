Counter logins = new Counter();
logins.Name = "logins";
logins.Increment();
logins.Increment();

Counter logouts = new Counter();
logouts.Name = "logouts";
logouts.Increment();

Counter unnamed = new Counter();

Console.WriteLine(logins.Describe());
Console.WriteLine(logouts.Describe());
Console.WriteLine(unnamed.Describe());

class Counter
{
    public string Name = "";
    public int Value;

    public void Increment()
    {
        Value++;
    }

    public string Describe()
    {
        return $"[{Name}] {Value}";
    }
}
