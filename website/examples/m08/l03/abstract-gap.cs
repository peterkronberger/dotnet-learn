Device device = new Scanner("archive scanner");
Console.WriteLine(device.Describe());

// Task 1: make the class abstract
class Device
{
    public string Name { get; }

    protected Device(string name)
    {
        Name = name;
    }

    // Task 2: make the method abstract, so it has no body
    public virtual string Describe() => $"Device: {Name}";
}

class Scanner : Device
{
    public Scanner(string name) : base(name) { }

    // Task 3: override Describe and return "Scanner: " and the name
}
