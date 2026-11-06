Printer printer = new("office printer");
Console.WriteLine($"Created: {printer.Name}");

class Device
{
    public string Name { get; }

    public Device(string name)
    {
        Console.WriteLine($"Device constructor: {name}");
        Name = name;
    }
}

class Printer : Device
{
    public Printer(string name) : base(name)
    {
        Console.WriteLine("Printer constructor");
    }
}
