Device device = new Printer("office printer", 40);
Device? missing = null;

Console.WriteLine(device is Printer);
Console.WriteLine(device is Scanner);
Console.WriteLine(missing is Device);

if (device is Printer printer)
{
    Console.WriteLine($"Pages per minute: {printer.PagesPerMinute}");
}

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

class Scanner : Device
{
    public Scanner(string name) : base(name) { }
}
