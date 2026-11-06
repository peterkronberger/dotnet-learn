Device device = new Scanner("archive scanner");

Printer? byAs = device as Printer;
Console.WriteLine(byAs is null);

try
{
    Printer byCast = (Printer)device;
    Console.WriteLine(byCast.Name);
}
catch (InvalidCastException ex)
{
    Console.WriteLine(ex.GetType().Name);
}

Scanner scanner = (Scanner)device;
Console.WriteLine(scanner.Name);

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
