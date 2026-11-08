Printer printer = new("office printer", 40);
Console.WriteLine(printer.Name);
Console.WriteLine(printer.Describe());
Console.WriteLine(printer.PagesPerMinute);

class Device
{
    public string Name { get; }

    public Device(string name)
    {
        Name = name;
    }

    public string Describe() => $"Device: {Name}";
}

class Printer : Device
{
    public int PagesPerMinute { get; }

    public Printer(string name, int pagesPerMinute) : base(name)
    {
        PagesPerMinute = pagesPerMinute;
    }
}
