Printer printer = new("office printer");
Device plain = new("router");

Console.WriteLine(printer);
Console.WriteLine($"Text: {printer}");
Console.WriteLine(plain.ToString());

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

    public override string ToString() => $"Printer {Name}";
}
