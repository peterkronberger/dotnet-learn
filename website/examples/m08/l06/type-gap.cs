Device[] devices =
[
    new Printer("office printer", 40),
    new Scanner("archive scanner"),
];
int scanners = 0;

foreach (Device device in devices)
{
    Console.WriteLine(device.Name);
    // Task 1: if device is a Printer, print its PagesPerMinute
    // Task 2: use as to count the scanners in the variable scanners
}

Console.WriteLine($"Scanners: {scanners}");

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
