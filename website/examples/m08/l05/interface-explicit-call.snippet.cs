Printer printer = new("office printer");
Console.WriteLine(printer.Describe());

interface ILabeled
{
    string Describe();
}

class Printer : ILabeled
{
    public Printer(string label) { }

    string ILabeled.Describe() => "Printer";
}
