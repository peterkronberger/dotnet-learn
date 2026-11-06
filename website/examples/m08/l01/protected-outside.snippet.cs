Printer printer = new("office printer");
Console.WriteLine(printer.Kind);

class Device
{
    protected string Kind = "device";
}

class Printer : Device
{
    public Printer(string name) { }
}
