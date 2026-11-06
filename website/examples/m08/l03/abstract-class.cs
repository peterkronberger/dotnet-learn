List<Device> devices =
[
    new Printer("office printer"),
    new Scanner("archive scanner"),
];

foreach (Device device in devices)
{
    Console.WriteLine($"{device.Label()} {device.Describe()}");
}

abstract class Device
{
    public string Name { get; }

    protected Device(string name)
    {
        Name = name;
    }

    public string Label() => $"[{Name}]";

    public abstract string Describe();
}

class Printer : Device
{
    public Printer(string name) : base(name) { }

    public override string Describe() => "prints pages";
}

class Scanner : Device
{
    public Scanner(string name) : base(name) { }

    public override string Describe() => "scans documents";
}
