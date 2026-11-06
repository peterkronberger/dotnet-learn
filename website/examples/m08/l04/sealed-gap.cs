Device device = new Scanner("archive scanner");
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

// Task 1: seal the class so that nothing can derive from it
class Scanner : Device
{
    public Scanner(string name) : base(name) { }

    // Task 2: seal this override
    public override string Describe() => $"Scanner: {Name}";
}
