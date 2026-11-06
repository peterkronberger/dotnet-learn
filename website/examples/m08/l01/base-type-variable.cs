List<Device> devices =
[
    new Device("router"),
    new Printer("office printer", 40),
];

foreach (Device device in devices)
{
    Console.WriteLine(device.Describe());
}

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
