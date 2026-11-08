Device device = new Scanner("archive scanner");
Printer p = (Printer)device;

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
    public Printer(string name) : base(name) { }
}

class Scanner : Device
{
    public Scanner(string name) : base(name) { }
}
