Counter counter = new Counter("logins");
counter.Add();
counter.Add();
Console.WriteLine(counter.Describe());

class Counter(string name)
{
    private int value;

    public void Add()
    {
        value++;
    }

    public string Describe()
    {
        return $"{name}: {value}";
    }
}
