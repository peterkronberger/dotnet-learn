List<ILabeled> items = [new Printer("office printer"), new Ticket(7)];

foreach (ILabeled item in items)
{
    Console.WriteLine(item.Describe());
}

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

    public string Describe() => $"Printer: {Label}";
}

class Ticket : ILabeled
{
    private readonly int _number;

    public Ticket(int number)
    {
        _number = number;
    }

    public string Label => $"ticket-{_number}";

    public string Describe() => $"Ticket: {Label}";
}
