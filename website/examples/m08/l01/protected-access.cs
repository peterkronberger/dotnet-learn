Printer printer = new("office printer");
Console.WriteLine(printer.Label());
Console.WriteLine(printer.SecretLength());

class Device
{
    private string secret = "hidden";
    protected string Kind = "device";

    public string Name { get; }

    public Device(string name)
    {
        Name = name;
    }

    public int SecretLength() => secret.Length;
}

class Printer : Device
{
    public Printer(string name) : base(name) { }

    public string Label() => $"{Kind}: {Name}";
}
