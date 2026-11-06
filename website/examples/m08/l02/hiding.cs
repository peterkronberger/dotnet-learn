Printer printer = new();
Device device = printer;

Console.WriteLine($"Overridden via Printer: {printer.Overridden()}");
Console.WriteLine($"Overridden via Device:  {device.Overridden()}");
Console.WriteLine($"Hidden via Printer:     {printer.Hidden()}");
Console.WriteLine($"Hidden via Device:      {device.Hidden()}");

class Device
{
    public virtual string Overridden() => "Device version";

    public virtual string Hidden() => "Device version";
}

class Printer : Device
{
    public override string Overridden() => "Printer version";

    public new string Hidden() => "Printer version";
}
