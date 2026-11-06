Device device = new Printer("office printer", 40);
Console.WriteLine(device.PagesPerMinute);

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
