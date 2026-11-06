Printer printer = new("office printer");
ILabeled labeled = printer;

Console.WriteLine(labeled.Describe());
Console.WriteLine(printer.Label);

interface ILabeled
{
    string Label { get; }

    string Describe();
}

class Printer : ILabeled
{
    public string Label { get; }

    public Printer(string label)
    {
        Label = label;
    }

    string ILabeled.Describe() => $"Printer: {Label}";
}
