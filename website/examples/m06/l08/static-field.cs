Ticket first = new Ticket("Many failed logins");
Ticket second = new Ticket("Disk full");
Console.WriteLine($"{first.Title} has number {first.Number}");
Console.WriteLine($"{second.Title} has number {second.Number}");
Console.WriteLine($"Tickets created: {Ticket.Created}");

class Ticket
{
    private static int created;

    public string Title { get; }
    public int Number { get; } = ++created;

    public static int Created => created;

    public Ticket(string title)
    {
        Title = title;
    }
}
