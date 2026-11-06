Printer printer = new() { Name = "office printer" };
Console.WriteLine(printer.Name);

printer.Name = "label printer";
Console.WriteLine(printer.Name);

Console.WriteLine($"Device sealed: {typeof(Device).IsSealed}");
Console.WriteLine($"Printer sealed: {typeof(Printer).IsSealed}");

class Device
{
}

sealed class Printer : Device
{
    public string Name { get; set; } = "";
}
