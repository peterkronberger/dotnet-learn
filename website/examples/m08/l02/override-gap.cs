Device device = new Scanner("archive scanner", 600);
Console.WriteLine(device.Describe());

class Device
{
    public string Name { get; }

    public Device(string name)
    {
        Name = name;
    }

    // Task 1: change this method so that derived classes can override it
    public string Describe() => $"Device: {Name}";
}

class Scanner : Device
{
    public int Dpi { get; }

    public Scanner(string name, int dpi) : base(name)
    {
        Dpi = dpi;
    }

    // Task 2: override Describe, call the base version, add the resolution
}
