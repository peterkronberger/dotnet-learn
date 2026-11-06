List<Device> devices =
[
    new Device("router"),
    new Printer("office printer"),
    new Scanner("archive scanner"),
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

    public virtual string Describe() => $"Device: {Name}";
}

class Printer : Device
{
    public Printer(string name) : base(name) { }

    public override string Describe() => $"Printer: {Name}";
}

class Scanner : Device
{
    public Scanner(string name) : base(name) { }
}
