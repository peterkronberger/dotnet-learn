Printer printer = new("office printer", 40);
Device device = printer;
Printer again = (Printer)device;
Console.WriteLine(object.ReferenceEquals(printer, again));

class Device
{
    public string Name { get; }

    public Device(string name)
    {
        Name = name;
    }
}

class Printer : Device
{
    public int PagesPerMinute { get; }

    public Printer(string name, int pagesPerMinute) : base(name)
    {
        PagesPerMinute = pagesPerMinute;
    }
}
