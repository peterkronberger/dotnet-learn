Device device = new Printer("office printer", 40);
Console.WriteLine(device.Describe());

class Device
{
    public string Name { get; }

    public Device(string name)
    {
        Name = name;
    }

    public virtual string Describe() => $"Device: {Name}";
}

class Printer : Device
{
    public int PagesPerMinute { get; }

    public Printer(string name, int pagesPerMinute) : base(name)
    {
        PagesPerMinute = pagesPerMinute;
    }

    public override string Describe()
    {
        return base.Describe() + $", {PagesPerMinute} pages per minute";
    }
}
