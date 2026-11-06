Folder folder = new();
folder.Add(new Printer("office printer"));
folder.Add(new Ticket(7));
folder.PrintAll();

class Folder
{
    private readonly List<ILabeled> _items = [];

    public void Add(ILabeled item)
    {
        _items.Add(item);
    }

    public void PrintAll()
    {
        foreach (ILabeled item in _items)
        {
            Console.WriteLine(item.Label);
        }
    }
}

interface ILabeled
{
    string Label { get; }
}

class Printer : ILabeled
{
    public string Label { get; }

    public Printer(string label)
    {
        Label = label;
    }
}

class Ticket : ILabeled
{
    private readonly int _number;

    public Ticket(int number)
    {
        _number = number;
    }

    public string Label => $"ticket-{_number}";
}
