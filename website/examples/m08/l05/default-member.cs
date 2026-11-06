ILabeled item = new Ticket();
Console.WriteLine(item.Describe());

interface ILabeled
{
    string Label { get; }

    string Describe() => $"Item: {Label}";
}

class Ticket : ILabeled
{
    public string Label => "ticket-7";
}
